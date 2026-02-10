using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence;
using Hiberus.Industria.Templates.Aspire.React.ServiceDefaults;

namespace Hiberus.Industria.Templates.Aspire.React.Server.MigrationService;

/// <summary>
/// Entry point for the migration service application.
/// Registers and runs the hosted background service responsible for database-related tasks, such as migrations.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Main method that sets up and runs the migration service as a hosted background task.
    /// </summary>
    /// <param name="args">Arguments from the command line.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task Main(string[] args)
    {
        // Create a HostApplicationBuilder to configure services and build the host
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();

        // Add infrastructure services
        builder.Services.AddPersistence(builder.Configuration);

        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        // Register the Worker class as a hosted background service
        builder.Services.AddHostedService<Worker>();

        builder
            .Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

        // Build and run the host
        IHost host = builder.Build();
        await host.RunAsync().ConfigureAwait(false);
    }
}
