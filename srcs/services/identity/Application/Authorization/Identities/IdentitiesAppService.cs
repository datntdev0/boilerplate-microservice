using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

public class IdentitiesAppService(IServiceProvider services) : BaseAppService, IIdentitiesAppService
{
    private readonly IdentitiesManager _manager = services.GetRequiredService<IdentitiesManager>();
    private readonly UsersManager _userManager = services.GetRequiredService<UsersManager>();
    private readonly HttpContext _httpContext = services.GetRequiredService<IHttpContextAccessor>().HttpContext!;

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

    [Route("session")]
    public async Task<SessionDto> GetSessionAsync()
    {
        if (_httpContext.User.Identity?.IsAuthenticated ?? false)
        {
            var emailAddress = _httpContext.User.GetClaim(Claims.Email);
            if (string.IsNullOrEmpty(emailAddress)) return new SessionDto();

            var userEntity = await _userManager.GetAsync(emailAddress);
            return new SessionDto() { User = Map<SessionUserDto>(userEntity) };
        }
        return new SessionDto();
    }
}
