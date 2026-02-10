using System.Reflection;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application;

/// <summary>
/// Extension methods for registering Application layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds MediatR core services to the application.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddApplicationMediatRCore(this IServiceCollection services)
    {
        // Register pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(InfrastructureErrorBehavior<,>));
        return services;
    }

    /// <summary>
    /// Adds MediatR services from the specified assemblies.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="assemblies">The assemblies to scan for MediatR handlers.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddApplicationMediatRFromAssemblies(
        this IServiceCollection services,
        params Assembly[] assemblies
    )
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        return services;
    }
}
