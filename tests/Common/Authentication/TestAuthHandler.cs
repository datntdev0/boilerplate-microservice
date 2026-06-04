using datntdev.Microservice.Shared.Common.Helpers;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace datntdev.Microservice.Tests.Common.Authentication;

public class TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "TestAuth";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers.Authorization.ToString();
        var base64 = StringHelper.GetSubstring(authHeader, "TestKey");
        if (string.IsNullOrEmpty(base64)) return Task.FromResult(AuthenticateResult.NoResult());

        // Deserialize SessionDto
        var json = StringHelper.ConvertFromBase64(base64);
        var sessionDto = JsonHelper.Deserialize<SessionDto>(json);
        if (sessionDto?.User == null) return Task.FromResult(AuthenticateResult.NoResult());

        // Create claims from SessionDto
        var claims = new List<Claim>
        {
            new(Claims.Subject, sessionDto.User.Id.ToString()),
            new(Claims.Email, sessionDto.User.EmailAddress),
            new(Claims.Name, $"{sessionDto.User.FirstName} {sessionDto.User.LastName}")
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
