namespace Hiberus.Industria.Templates.Aspire.React.AppHost;

/// <summary>
/// Entry point for the distributed Aspire application.
/// Sets up infrastructure and projects for Templates Aspire React.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Configures and runs the Aspire distributed application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        // External dependencies - PostgreSQL
        IResourceBuilder<PostgresServerResource> database = builder
            .AddPostgres("database")
            .WithDataVolume("templates-aspire-react-database-data")
            .WithPgAdmin(options =>
            {
                options.WithHostPort(5050);
                options.WithLifetime(ContainerLifetime.Persistent);
                options.WithEnvironment("PGADMIN_CONFIG_UPGRADE_CHECK_ENABLED", "False");
            })
            .WithLifetime(ContainerLifetime.Persistent);

        IResourceBuilder<PostgresDatabaseResource> mainDatabase = database.AddDatabase(
            "templates-aspire-react-database"
        );

        // External dependencies - Keycloak
        IResourceBuilder<KeycloakResource> keycloak = builder
            .AddKeycloak("keycloak", 8080)
            .WithDataVolume("templates-aspire-react-keycloak-data")
            .WithRealmImport("../../configuration/keycloak/realms/master.json")
            .WithRealmImport(
                "../../configuration/keycloak/realms/templates-aspire-react-realm.json"
            )
            .WithLifetime(ContainerLifetime.Persistent);

        // Backend project
        IResourceBuilder<ProjectResource> server = builder
            .AddProject<Projects.Hiberus_Industria_Templates_Aspire_React_Server>("server")
            .WithReference(mainDatabase)
            .WithReference(keycloak)
            .WaitFor(mainDatabase)
            .WaitFor(keycloak);

        // Frontend project
        builder
            .AddViteApp("client", "../Services/Hiberus.Industria.Templates.Aspire.React.Client")
            .WithReference(server)
            .WaitFor(server);

        // Build and run the distributed application
        await builder.Build().RunAsync().ConfigureAwait(false);
    }
}
