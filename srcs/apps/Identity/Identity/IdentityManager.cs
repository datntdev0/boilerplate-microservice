using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Shared.Communication.HttpClients;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using IdentityResult = datntdev.Microservice.App.Identity.Models.IdentityResult;

namespace datntdev.Microservice.App.Identity.Identity;

public class IdentityManager(IServiceProvider services) 
    : MicroserviceAppIdentityBaseManager(services)
{
    private readonly IHttpContextAccessor _contextAccessor = services.GetRequiredService<IHttpContextAccessor>();
    private readonly SrvIdentityHttpClient _srvIdentityHttpClient = services.GetRequiredService<SrvIdentityHttpClient>();

    public async Task<IdentityResult> SignInWithPassword(string email, string password)
    {
        try
        {
            var userDto = await _srvIdentityHttpClient.Identities_CreateSigninAsync(
                new SigninDto { Email = email, Password = password });

            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new(ClaimTypes.Name, $"{userDto.FirstName} {userDto.LastName}"),
                new(ClaimTypes.Email, email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, Constants.Application.AuthenticationScheme);
            await _contextAccessor.HttpContext!.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            return IdentityResult.Success;
        }
        catch (ExceptionInternalApi)
        {
            return IdentityResult.Failure;
        }
    }

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
