using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities;

public interface IIdentitiesAppService
    : IAppService<long, IdentityDto, IdentityCreateDto, IdentityUpdateDto>
{
    Task<PaginatedResult<IdentityListDto>> GetAllAsync(PaginatedRequest request);
    Task<UserDto> CreateSigninAsync(SigninDto request);
    Task<UserDto> CreateSignupAsync(SignupDto request);
}
