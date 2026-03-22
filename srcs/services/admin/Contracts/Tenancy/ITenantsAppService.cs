using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;

namespace datntdev.Microservice.Srv.Admin.Contracts.Tenancy;

public interface ITenantsAppService
    : IAppService<int, TenantDto, TenantCreateDto, TenantUpdateDto>
{
    Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request);
}
