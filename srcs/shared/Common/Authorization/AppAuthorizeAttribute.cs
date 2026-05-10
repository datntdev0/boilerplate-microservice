using Microsoft.AspNetCore.Authorization;

namespace datntdev.Microservice.Shared.Common.Authorization;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AppAuthorizeAttribute(params Constants.Permissions[] permissions) : AuthorizeAttribute
{
    public Constants.Permissions[] Permissions { get; } = permissions ?? [];
}