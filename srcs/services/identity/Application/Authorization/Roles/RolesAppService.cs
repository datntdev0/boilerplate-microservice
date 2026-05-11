using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Authorization;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Roles.Entities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Roles.Dto;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Roles;

public class RolesAppService(IServiceProvider services) : BaseAppService, IRolesAppService
{
    private readonly RolesManager _manager = services.GetRequiredService<RolesManager>();
    private readonly RoleCreatingValidator _creatingValidator = services.GetRequiredService<RoleCreatingValidator>();
    private readonly RoleUpdatingValidator _updatingValidator = services.GetRequiredService<RoleUpdatingValidator>();

    [AppAuthorize(Constants.Permissions.Roles_Write)]
    public async Task<RoleDto> CreateAsync(RoleCreateDto request)
    {
        _creatingValidator.ValidateAndThrow(request);
        var entity = await _manager.CreateAsync(Map<RoleEntity>(request));
        return Map<RoleDto>(entity);
    }

    [AppAuthorize(Constants.Permissions.Roles_Write)]
    public Task DeleteAsync(int id)
    {
        return _manager.DeleteAsync(id);
    }

    [AppAuthorize(Constants.Permissions.Roles_Read)]
    public async Task<PaginatedResult<RoleListDto>> GetAllAsync(PaginatedRequest request)
    {
        var total = await _manager.Queryable.CountAsync();
        var items = await _manager.Queryable
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync();

        return new PaginatedResult<RoleListDto>()
        {
            Total = total,
            Items = items.Adapt<List<RoleListDto>>(),
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    [AppAuthorize(Constants.Permissions.Roles_Read)]
    public async Task<RoleDto> GetAsync(int id)
    {
        var entity = await _manager.GetAsync(id);
        return Map<RoleDto>(entity);
    }

    [AppAuthorize(Constants.Permissions.Roles_Write)]
    public async Task<RoleDto> UpdateAsync(int id, RoleUpdateDto request)
    {
        _updatingValidator.ValidateAndThrow(request);
        var entity = await _manager.GetAsync(id);
        MapTo(request, entity);
        entity = await _manager.UpdateAsync(entity);
        return Map<RoleDto>(entity);
    }
}
