namespace SoccerManager.API.Models;

/// <summary>
/// A single day's weather forecast.
/// </summary>
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    /// <summary>
    /// The temperature in degrees Fahrenheit, converted from <see cref="TemperatureC"/>.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
