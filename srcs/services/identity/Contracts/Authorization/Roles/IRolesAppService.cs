using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles;

public interface IRolesAppService
    : IAppService<int, RoleDto, RoleCreateDto, RoleUpdateDto>
{
    Task<PaginatedResult<RoleListDto>> GetAllAsync(PaginatedRequest request);
}
