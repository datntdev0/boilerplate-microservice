using datntdev.Microservice.Shared.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Microservice.Shared.Common.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AppAuthorizationAttribute(params Constants.Permissions[] permissions) : Attribute, IAuthorizationFilter
{
    public Constants.Permissions[] Permissions { get; } = permissions ?? [];

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;

        if (isAuthenticated && Permissions.Length == 0) return;

        throw new ExceptionUnauthorized("Authentication is required to access this resource.");

        // User is authenticated - allow access
        // Permission validation intentionally skipped for now (per user request)
        // The Permissions array is stored for future implementation
        
        // Future: Add permission validation logic here
        // Example:
        // if (Permissions.Length > 0)
        // {
        //     var userPermissions = user.Claims
        //         .Where(c => c.Type == "permission")
        //         .Select(c => Enum.Parse<Constants.Permissions>(c.Value));
        //     
        //     if (!Permissions.Any(p => userPermissions.Contains(p)))
        //     {
        //         context.Result = new ObjectResult(new ErrorResponse
        //         {
        //             StatusCode = StatusCodes.Status403Forbidden,
        //             ErrorCode = 403,
        //             Message = "You do not have the required permissions."
        //         })
        //         {
        //             StatusCode = StatusCodes.Status403Forbidden
        //         };
        //     }
        // }
    }
}