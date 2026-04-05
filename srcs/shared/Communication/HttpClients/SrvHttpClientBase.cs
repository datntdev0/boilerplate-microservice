using System.Text.Json;

namespace datntdev.Microservice.Shared.Communication.HttpClients;

public abstract class SrvHttpClientBase
{
    protected static void UpdateJsonSerializerSettings(JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true; // Allow case-insensitive property matching
    }
}
