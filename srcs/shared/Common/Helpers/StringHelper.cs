using System.Net.Http.Headers;
using System.Text;

namespace datntdev.Microservice.Shared.Common.Helpers;

public static class StringHelper
{
    public static string ConvertFromBase64(string base64String)
    {
        if (string.IsNullOrEmpty(base64String)) return string.Empty;

        var bytes = Convert.FromBase64String(base64String);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string ConvertToBase64(string plainString)
    {
        if (string.IsNullOrEmpty(plainString)) return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(plainString);
        return Convert.ToBase64String(bytes);
    }

    public static string? GetSubstring(string? input, string? prefix)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(prefix)) return input;
        return input.StartsWith(prefix, StringComparison.InvariantCulture) ?
            input[prefix.Length..].Trim() : input;
    }

    public static AuthenticationHeaderValue? ParseAuthenticationHeader(string? headerValue)
    {
        if (string.IsNullOrEmpty(headerValue)) return null;

        var parts = headerValue.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return null;

        return new AuthenticationHeaderValue(parts[0], parts[1]);
    }
}