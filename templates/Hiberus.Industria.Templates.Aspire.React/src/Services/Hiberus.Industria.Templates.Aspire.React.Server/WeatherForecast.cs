namespace Hiberus.Industria.Templates.Aspire.React.Server;

/// <summary>
/// Represents a weather forecast with date, temperature in Celsius, and a summary description.
/// </summary>
/// <param name="Date">The date of the forecast.</param>
/// <param name="TemperatureC">The temperature in Celsius.</param>
/// <param name="Summary">A brief summary of the weather conditions.</param>
/// <remarks>
/// This record is used to model the weather forecast data returned by the API endpoint.
/// The TemperatureF property is calculated based on the TemperatureC value for convenience.
/// </remarks>
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    /// <summary>
    /// Gets the temperature in Fahrenheit, calculated from the Celsius value.
    /// </summary>
    public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);
}
