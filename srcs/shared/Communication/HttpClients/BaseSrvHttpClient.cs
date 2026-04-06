using System.Text.Json;

namespace datntdev.Microservice.Shared.Communication.HttpClients;

public abstract class BaseSrvHttpClient
{
    protected static void UpdateJsonSerializerSettings(JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true; // Allow case-insensitive property matching
    }
}
