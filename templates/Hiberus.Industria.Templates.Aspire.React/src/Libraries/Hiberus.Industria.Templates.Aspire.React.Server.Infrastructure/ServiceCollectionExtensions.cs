using Duende.AccessTokenManagement;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Identity;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Common;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Identity;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence;
using Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence.Repositories;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure;

/// <summary>
/// Extension methods for registering Infrastructure layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddInfrastructureCore(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddScoped<IClaimsAccessor, ClaimsAccessor>();
        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }

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

    /// <summary>
    /// Adds JWT Bearer authentication to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration to read settings from.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddJwtBearerAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ArgumentNullException.ThrowIfNull(configuration);

        // Configure options using Options pattern - single source of truth
        IConfigurationSection oAuthConfig = configuration.GetSection("Authentication:OAuth");
        services
            .AddOptions<OAuthOptions>()
            .Bind(oAuthConfig)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Using a service provider to resolve options
        OAuthOptions oAuthOptions =
            configuration.GetSection("Authentication:OAuth").Get<OAuthOptions>()
            ?? throw new InvalidOperationException("Missing OAuth configuration section.");

        services
            .AddAuthentication()
            .AddKeycloakJwtBearer(
                "keycloak",
                "templates-aspire-react",
                options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "server";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuers = oAuthOptions.ValidIssuers,
                    };
                }
            );

        return services;
    }

    /// <summary>
    /// Adds custom authorization policies to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddKeycloakAuthorization(options =>
                options.EnableRolesMapping = RolesClaimTransformationSource.Realm
            )
            .AddAuthorizationBuilder()
            .AddPolicy(
                PolicyNames.HasOperatorRole,
                policy => policy.RequireRealmRoles(RoleNames.Operator)
            )
            .AddPolicy(
                PolicyNames.HasAdministratorRole,
                policy => policy.RequireRealmRoles(RoleNames.Administrator)
            )
            .AddPolicy(
                PolicyNames.HasOperatorOrAdminRole,
                policy => policy.RequireRealmRoles(RoleNames.All)
            );

        return services;
    }

    /// <summary>
    /// Adds Keycloak admin client services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration to read settings from.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddKeycloakAdminClient(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ArgumentNullException.ThrowIfNull(configuration);

        // Configure options using Options pattern - single source of truth
        IConfigurationSection keycloakAdminSection = configuration.GetSection(
            "Authentication:KeycloakClient"
        );
        services
            .AddOptions<KeycloakClientOptions>()
            .Bind(keycloakAdminSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Using a service provider to resolve options
        KeycloakClientOptions keycloakClientOptions =
            configuration.GetSection("Authentication:KeycloakClient").Get<KeycloakClientOptions>()
            ?? throw new InvalidOperationException("Missing Keycloak configuration section.");

        var clientId = ClientId.Parse(keycloakClientOptions.ClientId);
        var clientSecret = ClientSecret.Parse(keycloakClientOptions.ClientSecret);

        var tokenClientName = ClientCredentialsClientName.Parse("keycloak-admin-client");

        services
            .AddClientCredentialsTokenManagement()
            .AddClient(
                tokenClientName,
                options =>
                {
                    options.TokenEndpoint = keycloakClientOptions.TokenUrl;
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                }
            );

        // Decouple Keycloak client from Keycloak Admin Client options
        KeycloakAdminClientOptions keycloakAdminClientOptions = new()
        {
            AuthServerUrl = keycloakClientOptions.AuthServerUrl.ToString(),
            Realm = keycloakClientOptions.Realm,
            Resource = keycloakClientOptions.ClientId,
        };

        services
            .AddKeycloakAdminHttpClient(keycloakAdminClientOptions)
            .AddClientCredentialsTokenHandler(tokenClientName)
            .AddStandardResilienceHandler();

        return services;
    }
}
