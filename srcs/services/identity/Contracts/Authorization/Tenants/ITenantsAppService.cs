using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants;

public interface ITenantsAppService : IAppService
{
    Task<PaginatedResult<TenantUserListDto>> GetAllAsync(long id, PaginatedRequest request);
    Task<TenantUsersInviteResultDto> CreateTenantUsersAsync(long id, TenantUsersInviteDto request);
    Task PatchTenantUsersAsync(long id, TenantUsersPatchDto request);
}
