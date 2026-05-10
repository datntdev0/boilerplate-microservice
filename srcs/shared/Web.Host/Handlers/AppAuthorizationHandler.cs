using datntdev.Microservice.Shared.Application.Authorization;
using datntdev.Microservice.Shared.Common.Authorization;
using datntdev.Microservice.Shared.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Web.Host.Handlers;

public class AuthorizationMiddlewareHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false)) throw ExceptionUnauthorized.Default();

        var sessionAppProvider = context.RequestServices.GetRequiredService<SessionAppProvider>();
        var permissions = (await sessionAppProvider.GetSessionAsync()).User!.Permissions.ToHashSet();
        var authorizeAttributes = context.GetEndpoint()!.Metadata.GetOrderedMetadata<AppAuthorizeAttribute>();

        // Checking if the user has any of the required permissions for all AppAuthorizeAttributes
        var accessGranted = authorizeAttributes.All(a => a.Permissions.Any(p => permissions.Contains(p)));

        if (!accessGranted) throw new ExceptionForbidden("You do not have permission to access this resource.");

        await next(context);
    }
}