using System.Text.Json;

namespace datntdev.Microservice.Shared.Common.Helpers;

public static class JsonHelper
{
    public static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
    };

    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, DefaultOptions)!;
    }

    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, DefaultOptions);
    }
}