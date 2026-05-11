using datntdev.Microservice.Shared.Application.Authorization;
using datntdev.Microservice.Shared.Common;
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
        // Step 1: Verify user is authenticated
        if (!(context.User.Identity?.IsAuthenticated ?? false)) 
            throw ExceptionUnauthorized.Default();

        var sessionAppProvider = context.RequestServices.GetRequiredService<SessionAppProvider>();
        var session = await sessionAppProvider.GetSessionAsync();
        
        // Step 2: Build effective permission set from user's direct permissions + all assigned role permissions
        var effectivePermissions = new HashSet<Constants.Permissions>(session.User!.Permissions ?? []);
        if (session.User.Roles != null)
        {
            foreach (var role in session.User.Roles)
            {
                foreach (var permission in role.Permissions ?? [])
                {
                    effectivePermissions.Add(permission);
                }
            }
        }

        var authorizeAttributes = context.GetEndpoint()!.Metadata.GetOrderedMetadata<AppAuthorizeAttribute>();

        // Step 3: Evaluate authorization requirements
        // Multiple attributes are combined with AND (all attributes must pass)
        // Multiple permissions within one attribute are combined with OR (any one permission suffices)
        // Empty permission list on an attribute means authenticated-only (already verified in Step 1)
        var accessGranted = authorizeAttributes.All(attribute =>
        {
            // If no permissions are specified, just being authenticated is enough
            if (attribute.Permissions.Length == 0)
                return true;

            // Check if user has any of the required permissions (OR logic)
            return attribute.Permissions.Any(requiredPermission =>
            {
                // Direct permission match
                if (effectivePermissions.Contains(requiredPermission))
                    return true;

                // Parent-implies-child: parent permission satisfies child requirements
                // E.g., Users=2000 satisfies Users_Read=2001 and Users_Write=2002
                // Calculated as: parent = requiredPermission - (requiredPermission % 1000)
                var parentPermission = (Constants.Permissions)((int)requiredPermission - ((int)requiredPermission % 1000));
                if (parentPermission != Constants.Permissions.None && effectivePermissions.Contains(parentPermission))
                    return true;

                return false;
            });
        });

        if (!accessGranted) 
            throw new ExceptionForbidden("You do not have permission to access this resource.");

        await next(context);
    }
}