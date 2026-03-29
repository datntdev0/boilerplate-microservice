using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users;

public class UsersAppService(IServiceProvider services) : BaseAppService, IUsersAppService
{
    private readonly UsersManager _manager = services.GetRequiredService<UsersManager>();
    private readonly UserCreatingValidator _creatingValidator = services.GetRequiredService<UserCreatingValidator>();
    private readonly UserUpdatingValidator _updatingValidator = services.GetRequiredService<UserUpdatingValidator>();

    public async Task<UserDto> CreateAsync(UserCreateDto request)
    {
        _creatingValidator.ValidateAndThrow(request);

        var entity = await _manager.CreateAsync(new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
        });
        return MapToDto(entity);
    }

    public Task DeleteAsync(long id)
    {
        return _manager.DeleteAsync(id);
    }

    public async Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request)
    {
        var total = await _manager.Queryable.CountAsync();
        var items = await _manager.Queryable
            .Skip(request.Offset)
            .Take(request.Limit)
            .Select(x => new UserListDto()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
            })
            .ToListAsync();
        return new PaginatedResult<UserListDto>()
        {
            Total = total,
            Items = items,
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    public async Task<UserDto> GetAsync(long id)
    {
        var entity = await _manager.GetAsync(id);
        return MapToDto(entity);
    }

    public async Task<UserDto> UpdateAsync(long id, UserUpdateDto request)
    {
        _updatingValidator.ValidateAndThrow(request);

        var entity = await _manager.GetAsync(id);
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;

        entity = await _manager.UpdateAsync(entity);
        return MapToDto(entity);
    }

    private static UserDto MapToDto(Authorization.Users.Entities.UserEntity entity) => new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };
}
