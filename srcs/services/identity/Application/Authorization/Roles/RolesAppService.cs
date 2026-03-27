using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Roles;

public class RolesAppService : BaseAppService, IRolesAppService
{
    public Task<RoleDto> CreateAsync(RoleCreateDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<RoleListDto>> GetAllAsync(PaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<RoleDto> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<RoleDto> UpdateAsync(long id, RoleUpdateDto request)
    {
        throw new NotImplementedException();
    }
}
