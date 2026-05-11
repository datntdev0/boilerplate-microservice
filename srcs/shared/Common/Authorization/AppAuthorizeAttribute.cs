using Microsoft.AspNetCore.Authorization;

namespace datntdev.Microservice.Shared.Common.Authorization;

/// <summary>
/// Specifies permission requirements for protecting API endpoints or methods.
/// This attribute integrates with the AppAuthorizationHandler to enforce permission-based access control.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AppAuthorizeAttribute(params Constants.Permissions[] permissions) : AuthorizeAttribute
{
    public Constants.Permissions[] Permissions { get; } = permissions ?? [];
}