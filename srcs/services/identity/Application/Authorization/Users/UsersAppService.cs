using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Authorization;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users;

public class UsersAppService(IServiceProvider services) : BaseAppService, IUsersAppService
{
    private readonly UsersManager _manager = services.GetRequiredService<UsersManager>();
    private readonly UserCreatingValidator _creatingValidator = services.GetRequiredService<UserCreatingValidator>();
    private readonly UserUpdatingValidator _updatingValidator = services.GetRequiredService<UserUpdatingValidator>();

    [AppAuthorize(Constants.Permissions.Users_Write)]
    public async Task<UserDto> CreateAsync(UserCreateDto request)
    {
        _creatingValidator.ValidateAndThrow(request);
        var entity = await _manager.CreateAsync(request);
        return Map<UserDto>(entity);
    }

    [AppAuthorize(Constants.Permissions.Users_Write)]
    public Task DeleteAsync(long id)
    {
        return _manager.DeleteAsync(id);
    }

    [AppAuthorize(Constants.Permissions.Users_Read)]
    public async Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request)
    {
        var total = await _manager.Queryable.CountAsync();
        var items = await _manager.Queryable
            .Include(x => x.Identities)
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync();

        return new PaginatedResult<UserListDto>()
        {
            Total = total,
            Items = items.Adapt<List<UserListDto>>(),
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    [AppAuthorize(Constants.Permissions.Users_Read)]
    public async Task<UserDto> GetAsync(long id)
    {
        var entity = await _manager.GetAsync(id);
        return Map<UserDto>(entity);
    }

    [AppAuthorize(Constants.Permissions.Users_Write)]
    public async Task<UserDto> UpdateAsync(long id, UserUpdateDto request)
    {
        request.Id = id;
        _updatingValidator.ValidateAndThrow(request);
        var entity = await _manager.UpdateAsync(request);
        return Map<UserDto>(entity);
    }
}
