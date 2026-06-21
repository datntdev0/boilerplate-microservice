using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Communication.Extensions;
using datntdev.Microservice.Shared.Communication.HttpClients;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

public class IdentitiesAppService(IServiceProvider services) : BaseAppService, IIdentitiesAppService
{
    private readonly IdentitiesManager _manager = services.GetRequiredService<IdentitiesManager>();
    private readonly UsersManager _userManager = services.GetRequiredService<UsersManager>();
    private readonly HttpContext _httpContext = services.GetRequiredService<IHttpContextAccessor>().HttpContext!;
    private readonly ISrvAdminHttpClient _srvAdminHttpClientApiKey = services.GetRequiredHttpProxyService<ISrvAdminHttpClient>(true);
    private readonly ILogger<IdentitiesAppService> _logger = services.GetRequiredService<ILogger<IdentitiesAppService>>();

    [AppRoute("signin")]
    public async Task<UserDto> CreateSigninAsync(SigninDto request)
    {
        var user = await _manager.SigninAsync(request.Email, request.Password);
        return Map<UserDto>(user);
    }

    [AppRoute("signup")]
    public async Task<UserDto> CreateSignupAsync(SignupDto request)
    {
        var user = await _manager.SignupAsync(request.Email, request.Password, request.FirstName, request.LastName);
        return Map<UserDto>(user);
    }

    [AppRoute("session")]
    public async Task<SessionDto> GetSessionAsync()
    {
        if (!(_httpContext.User.Identity?.IsAuthenticated ?? false))
            return new SessionDto();

        var emailAddress = _httpContext.User.GetClaim(Claims.Email);
        if (string.IsNullOrEmpty(emailAddress)) return new SessionDto();

        var userEntity = await _userManager.GetAsync(emailAddress);
        var sessionUser = Map<SessionUserDto>(userEntity);
        var tenantList = new List<SessionTenantDto>();

        if (userEntity.Roles.Any(r => r.TenantId == null))
            tenantList.Add(new SessionTenantDto { Id = null, Name = "Host" });

        var adminTenants = await _srvAdminHttpClientApiKey.Tenants_GetAllAsync(null, null);
        var assignedTenantIds = userEntity.Tenants.Select(t => t.TenantId).ToHashSet();
        var assignedTenants = adminTenants.Items
            .Where(t => assignedTenantIds.Contains(t.Id))
            .OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase)
            .Select(t => new SessionTenantDto { Id = t.Id, Name = t.Name });
        tenantList.AddRange(assignedTenants);

        sessionUser.Tenants = [.. tenantList];
        return new SessionDto { User = sessionUser };
    }
}
