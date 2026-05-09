using Microsoft.AspNetCore.Http;

namespace datntdev.Microservice.Shared.Communication.Handlers;

/// <summary>
/// HTTP message handler that propagates the Authorization header from the current HttpContext
/// to outgoing HTTP requests for inter-service communication.
/// </summary>
public class AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // If there's an active HTTP context and it contains an Authorization header,
        // propagate it to the outgoing request
        if (httpContext?.Request.Headers.TryGetValue("Authorization", out var authHeaderValue) == true)
        {
            if (authHeaderValue.Count > 0 && !string.IsNullOrWhiteSpace(authHeaderValue[0]))
            {
                request.Headers.TryAddWithoutValidation("Authorization", authHeaderValue.ToString());
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
