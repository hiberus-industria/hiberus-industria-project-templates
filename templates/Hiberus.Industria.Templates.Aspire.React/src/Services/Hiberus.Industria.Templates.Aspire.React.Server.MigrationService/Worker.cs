using System.Diagnostics;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hiberus.Industria.Templates.Aspire.React.Server.MigrationService;

/// <summary>
/// Background service responsible for executing periodic tasks,
/// such as database migrations or maintenance jobs.
/// </summary>
public class Worker : BackgroundService
{
    /// <summary>
    /// The name of the activity source used for telemetry.
    /// </summary>
    public const string ActivitySourceName = "Migrations";

    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    private readonly IServiceProvider serviceProvider;
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    /// <summary>
    /// Initializes a new instance of the <see cref="Worker"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="hostApplicationLifetime">The host application lifetime.</param>
    public Worker(
        IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime
    )
    {
        this.serviceProvider = serviceProvider;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using Activity? activity = ActivitySource.StartActivity(
            "Migrating database",
            ActivityKind.Client
        );

        try
        {
            AsyncServiceScope scope = this.serviceProvider.CreateAsyncScope();

            await using (scope.ConfigureAwait(false))
            {
                ApplicationDbContext dbContext =
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await RunMigrationAsync(dbContext, stoppingToken).ConfigureAwait(false);
            }
        }
        catch (Exception exception)
        {
            activity?.AddException(exception);
            throw;
        }
        finally
        {
            // Ensure the application stops gracefully after migration
            this.hostApplicationLifetime.StopApplication();
        }
    }

    private static async Task RunMigrationAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy
            .ExecuteAsync(async () =>
                await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false)
            )
            .ConfigureAwait(false);
    }
}
