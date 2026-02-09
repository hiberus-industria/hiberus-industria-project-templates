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

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddProblemDetails();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        WebApplication app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        string[] summaries =
        [
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching",
        ];
        RouteGroupBuilder api = app.MapGroup("/api");
        api.MapGet(
                "weatherforecast",
                () =>
                {
                    return Enumerable
                        .Range(1, 5)
                        .Select(index =>
                        {
                            DateTime date = DateTime.Now.AddDays(index);
                            var forecastDate = DateOnly.FromDateTime(date);
                            int temperature = Random.Shared.Next(-20, 55);
                            string summary = summaries[Random.Shared.Next(summaries.Length)];
                            return new WeatherForecast(forecastDate, temperature, summary);
                        })
                        .ToArray();
                }
            )
            .WithName("GetWeatherForecast");

        await app.RunAsync().ConfigureAwait(false);
    }
}
