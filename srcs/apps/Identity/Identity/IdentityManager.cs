using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Shared.Communication.HttpClients;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using IdentityResult = datntdev.Microservice.App.Identity.Models.IdentityResult;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace datntdev.Microservice.App.Identity.Identity;

public class IdentityManager(IServiceProvider services) 
    : MicroserviceAppIdentityBaseManager(services)
{
    private readonly IHttpContextAccessor _contextAccessor = services.GetRequiredService<IHttpContextAccessor>();
    private readonly SrvIdentityHttpClient _srvIdentityHttpClient = services.GetRequiredService<SrvIdentityHttpClient>();

    /// <summary>
    /// Validates user credentials against the Identity Service.
    /// Returns the user DTO if valid, throws ExceptionInternalApi if invalid.
    /// </summary>
    public async Task<UserDto> ValidateCredentialsAsync(string email, string password)
    {
        var userDto = await _srvIdentityHttpClient.Identities_CreateSigninAsync(
            new SigninDto { Email = email, Password = password });
        return userDto;
    }

    /// <summary>
    /// Creates claims from user DTO for use in authentication.
    /// </summary>
    public static Claim[] CreateClaimsFromUser(UserDto userDto, string email)
    {
        return
        [
            new(Claims.Subject, userDto.Id.ToString()),
            new(Claims.Name, $"{userDto.FirstName} {userDto.LastName}"),
            new(Claims.Email, email),
        ];
    }

    /// <summary>
    /// Signs in a user with email and password.
    /// Validates credentials, creates claims, and signs in using cookie authentication.
    /// Returns IdentityResult indicating success or failure.
    /// </summary>
    public async Task<IdentityResult> SignInWithPassword(string email, string password)
    {
        try
        {
            var userDto = await ValidateCredentialsAsync(email, password);
            var claims = CreateClaimsFromUser(userDto, email);

            var claimsIdentity = new ClaimsIdentity(claims, Constants.Application.AuthenticationScheme);
            await _contextAccessor.HttpContext!.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            return IdentityResult.Success;
        }
        catch (ExceptionInternalApi)
        {
            return IdentityResult.Failure;
        }
    }

    /// <summary>
    /// Signs up a new user with email, password, first name, and last name.
    /// Returns IdentityResult indicating success or if the email is already in use.
    /// </summary>
    public async Task<IdentityResult> SignUpWithPassword(string email, string password, string firstName, string lastName)
    {
        try
        {
            await _srvIdentityHttpClient.Identities_CreateSignupAsync(
                new SignupDto { Email = email, Password = password, FirstName = firstName, LastName = lastName });

            return IdentityResult.Success;
        }
        catch (ExceptionInternalApi)
        {
            return IdentityResult.Duplicated;
        }
    }
}
