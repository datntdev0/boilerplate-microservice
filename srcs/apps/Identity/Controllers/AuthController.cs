using datntdev.Microservice.Shared.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace datntdev.Microservice.App.Identity.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet(Constants.Endpoints.AuthSignOut)]
    public async Task<IActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync(Constants.Application.AuthenticationScheme);
        return Redirect("/");
    }
}
