using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Roles;

public class RolesAppService(IServiceProvider services) : BaseAppService, IRolesAppService
{
    private readonly RolesManager _manager = services.GetRequiredService<RolesManager>();
    private readonly RoleCreatingValidator _creatingValidator = services.GetRequiredService<RoleCreatingValidator>();
    private readonly RoleUpdatingValidator _updatingValidator = services.GetRequiredService<RoleUpdatingValidator>();

    public async Task<RoleDto> CreateAsync(RoleCreateDto request)
    {
        _creatingValidator.ValidateAndThrow(request);

        var entity = await _manager.CreateAsync(new()
        {
            Name = request.Name,
            Description = request.Description
        });
        return MapToDto(entity);
    }

    public Task DeleteAsync(int id)
    {
        return _manager.DeleteAsync(id);
    }

    public async Task<PaginatedResult<RoleListDto>> GetAllAsync(PaginatedRequest request)
    {
        var total = await _manager.Queryable.CountAsync();
        var items = await _manager.Queryable
            .Skip(request.Offset)
            .Take(request.Limit)
            .Select(x => new RoleListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToListAsync();
        return new PaginatedResult<RoleListDto>()
        {
            Total = total,
            Items = items,
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    public async Task<RoleDto> GetAsync(int id)
    {
        var entity = await _manager.GetAsync(id);
        return MapToDto(entity);
    }

    public async Task<RoleDto> UpdateAsync(int id, RoleUpdateDto request)
    {
        _updatingValidator.ValidateAndThrow(request);

        var entity = await _manager.GetAsync(id);
        entity.Name = request.Name;
        entity.Description = request.Description;

        entity = await _manager.UpdateAsync(entity);
        return MapToDto(entity);
    }

    private static RoleDto MapToDto(Authorization.Roles.Entities.RoleEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };
}
