using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities;

public interface IIdentitiesAppService
    : IAppService<long, IdentityDto, IdentityCreateDto, IdentityUpdateDto>
{
    Task<PaginatedResult<IdentityListDto>> GetAllAsync(PaginatedRequest request);
}
