using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

public class IdentitiesAppService(IServiceProvider services) : BaseAppService, IIdentitiesAppService
{
    private readonly IdentitiesManager _manager = services.GetRequiredService<IdentitiesManager>();

    [Route("signin")]
    public async Task<UserDto> CreateSigninAsync(SigninDto request)
    {
        var user = await _manager.SigninAsync(request.Email, request.Password);
        return Map<UserDto>(user);
    }

    [Route("signup")]
    public async Task<UserDto> CreateSignupAsync(SignupDto request)
    {
        var user = await _manager.SignupAsync(request.Email, request.Password, request.FirstName, request.LastName);
        return Map<UserDto>(user);
    }
}
