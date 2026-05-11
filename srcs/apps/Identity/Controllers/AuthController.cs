using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.App.Identity.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace datntdev.Microservice.App.Identity.Controllers;

[ApiController]
public class AuthController(IdentityManager identityManager) : ControllerBase
{
    private readonly IdentityManager _identityManager = identityManager;
    private readonly string _authenticationScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

    private static void SetClaimsDestinations(ClaimsPrincipal claimsPrincipal)
    {
        claimsPrincipal.SetDestinations(static claim => claim.Type switch
        {
            Claims.Name => [Destinations.AccessToken, Destinations.IdentityToken],
            Claims.Email => [Destinations.AccessToken, Destinations.IdentityToken],
            _ => [],
        });
    }

    [HttpGet(Constants.Endpoints.OAuth2Authorize)]
    [HttpPost(Constants.Endpoints.OAuth2Authorize)]
    public async Task<IActionResult> AuthorizeAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Retrieve the user principal stored in the authentication cookie.
        var result = await HttpContext.AuthenticateAsync();

        // If the user principal can't be extracted, redirect the user to the login page.
        if (!result.Succeeded) return Challenge();

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(result.Principal.Claims, _authenticationScheme));

        // Set requested scopes (this is not done automatically)
        claimsPrincipal.SetScopes(request.GetScopes());
        SetClaimsDestinations(claimsPrincipal);

        // Signing in with the OpenIddict authentiction scheme trigger
        // OpenIddict to issue a code (which can be exchanged for an access token)
        return SignIn(claimsPrincipal, _authenticationScheme);
    }

    [HttpPost(Constants.Endpoints.OAuth2Token)]
    public async Task<IActionResult> TokenAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        ClaimsPrincipal claimsPrincipal;

        if (request.IsAuthorizationCodeGrantType())
        {
            // Retrieve the claims principal stored in the authorization code
            claimsPrincipal = (await HttpContext.AuthenticateAsync(_authenticationScheme)).Principal ??
                throw new InvalidOperationException("Can't retrieve the claims principal stored in the authorization code");
        }
        else if (request.IsClientCredentialsGrantType())
        {
            var claims = new Claim[] { new(Claims.Subject, "Service Principal") };
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, _authenticationScheme));
        }
        else if (request.IsPasswordGrantType())
        {
            // Resource Owner Password Credentials flow for headless/testing scenarios
            var email = request.Username!; var password = request.Password!;
            var userDto = await _identityManager.ValidateCredentialsAsync(email, password);

            // Create claims principal for the authenticated user
            var claims = IdentityManager.CreateClaimsFromUser(userDto, email);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, _authenticationScheme));
            
            // Set requested scopes (this is not done automatically)
            claimsPrincipal.SetScopes(request.GetScopes());
            SetClaimsDestinations(claimsPrincipal);
        }
        else
        {
            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(claimsPrincipal, _authenticationScheme);
    }

    [HttpGet(Constants.Endpoints.AuthSignOut)]
    public async Task<IActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync(Constants.Application.AuthenticationScheme);
        return SignOut(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
