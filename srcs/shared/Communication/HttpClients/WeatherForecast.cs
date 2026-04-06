namespace datntdev.Microservice.Shared.Communication.HttpClients;

/// <summary>
/// Sample WeatherForecast DTO used by default template controllers.
/// This should be removed in production once real endpoints are implemented.
/// </summary>
public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
