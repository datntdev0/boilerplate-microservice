using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Authorization;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Tenants;

public class TenantsAppService(IServiceProvider services) : BaseAppService, ITenantsAppService
{
    private readonly TenantsManager _manager = services.GetRequiredService<TenantsManager>();

    [AppAuthorize(Constants.Permissions.Tenancy_Read)]
    [AppRoute("{id}/users")]
    public async Task<PaginatedResult<TenantUserListDto>> GetAllAsync(long id, PaginatedRequest request)
    {
        var total = await _manager.CountByTenantAsync(id);
        var items = await _manager.GetByTenantAsync(id, request.Offset, request.Limit);

        return new PaginatedResult<TenantUserListDto>
        {
            Total = total,
            Items = items.Adapt<List<TenantUserListDto>>(),
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    [AppAuthorize(Constants.Permissions.Tenancy_Write)]
    [AppRoute("{id}/users")]
    public async Task<TenantUsersInviteResultDto> CreateTenantUsersAsync(long id, TenantUsersInviteDto request)
    {
        var matchedUserIds = await _manager.GetUserIdsByEmailsAsync(request.Emails);
        var matchedEmails = await _manager.GetEmailsByUserIdsAsync(matchedUserIds.ToArray());

        var recognizedEmails = request.Emails.Intersect(matchedEmails, StringComparer.OrdinalIgnoreCase).ToArray();
        var unrecognizedEmails = request.Emails.Except(matchedEmails, StringComparer.OrdinalIgnoreCase).ToArray();

        if (matchedUserIds.Count > 0)
        {
            await _manager.AssignUsersAsync(id, matchedUserIds);
        }

        return new TenantUsersInviteResultDto
        {
            RecognizedEmails = recognizedEmails,
            UnrecognizedEmails = unrecognizedEmails
        };
    }

    [AppAuthorize(Constants.Permissions.Tenancy_Write)]
    [AppRoute("{id}/users")]
    public async Task PatchTenantUsersAsync(long id, TenantUsersPatchDto request)
    {
        if (request.Create.Length > 0)
        {
            await _manager.AssignUsersAsync(id, request.Create);
        }

        if (request.Delete.Length > 0)
        {
            await _manager.RemoveUsersAsync(id, request.Delete);
        }
    }
}
