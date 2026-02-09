namespace Hiberus.Industria.Templates.Aspire.React.AppHost;

/// <summary>
/// Entry point for the distributed Aspire application.
/// Sets up infrastructure and projects for Volkswagen Picking.
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
        // Build and run the distributed application
        await builder.Build().RunAsync().ConfigureAwait(false);
    }
}
