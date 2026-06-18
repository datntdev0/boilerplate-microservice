using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants.Dto;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Tenants;

public interface ITenantsAppService : IAppService
{
    Task<PaginatedResult<TenantUserListDto>> GetAllAsync(int id, PaginatedRequest request);
    Task<TenantUsersInviteResultDto> CreateTenantUsersAsync(int id, TenantUsersInviteDto request);
    Task PatchTenantUsersAsync(int id, TenantUsersPatchDto request);
}
