using Hiberus.Industria.Templates.Aspire.React.Server.Application;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure;
using Hiberus.Industria.Templates.Aspire.React.ServiceDefaults;

namespace Hiberus.Industria.Templates.Aspire.React.Server;

/// <summary>
/// Entry point for the Volkswagen Picking Server application.
/// Configures services, middleware, and defines HTTP endpoints.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method that configures and runs the web application.
    /// </summary>
    /// <param name="args">Arguments from the command line.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Configure - OAuth options
        IConfigurationSection oAuthConfig = builder.Configuration.GetSection(
            "Authentication:OAuth"
        );
        builder.Services.Configure<OAuthOptions>(oAuthConfig);

        // Configure - Keycloak admin client options
        IConfigurationSection keycloakAdminConfig = builder.Configuration.GetSection(
            "Authentication:KeycloakClient"
        );
        builder.Services.Configure<KeycloakClientOptions>(keycloakAdminConfig);

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddProblemDetails();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        // Add application services
        builder
            .Services.AddApplicationMediatRCore()
            .AddApplicationMediatRFromAssemblies(
                typeof(Application.ServiceCollectionExtensions).Assembly
            );

        // Add infrastructure services
        builder
            .Services.AddInfrastructureCore()
            .AddPersistence(builder.Configuration)
            .AddRepositories()
            .AddJwtBearerAuthentication(builder.Configuration)
            .AddCustomAuthorization()
            .AddKeycloakAdminClient(builder.Configuration);

        WebApplication app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync().ConfigureAwait(false);
    }
}
