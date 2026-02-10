using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure;

/// <summary>
/// Extension methods for registering Infrastructure layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Entity Framework Core with PostgreSQL for the application database context.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration to read settings from.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ArgumentNullException.ThrowIfNull(configuration);

        // Configure options using Options pattern - single source of truth
        string connectionString =
            configuration.GetConnectionString("templates-aspire-react-database")
            ?? throw new InvalidOperationException(
                "Missing connection string 'templates-aspire-react-database'."
            );

        services.AddDbContext<ApplicationDbContext>(
            (_, options) => options.UseNpgsql(connectionString)
        );

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>()
        );

        return services;
    }

    /// <summary>
    /// Adds infrastructure repositories to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}
