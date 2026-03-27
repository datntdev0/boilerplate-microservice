using datntdev.Microservice.Shared.Common.Application;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

namespace datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users;

public interface IUsersAppService
    : IAppService<long, UserDto, UserCreateDto, UserUpdateDto>
{
    Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request);
}
