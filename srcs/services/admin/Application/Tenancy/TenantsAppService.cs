using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;

namespace datntdev.Microservice.Srv.Admin.Application.Tenancy;

public class TenantsAppService : BaseAppService, ITenantsAppService
{
    public Task<TenantDto> CreateAsync(TenantCreateDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<TenantDto> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TenantDto> UpdateAsync(int id, TenantUpdateDto request)
    {
        throw new NotImplementedException();
    }
}
