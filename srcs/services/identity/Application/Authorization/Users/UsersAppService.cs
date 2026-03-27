using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users;

public class UsersAppService : BaseAppService, IUsersAppService
{
    public Task<UserDto> CreateAsync(UserCreateDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateAsync(long id, UserUpdateDto request)
    {
        throw new NotImplementedException();
    }
}
