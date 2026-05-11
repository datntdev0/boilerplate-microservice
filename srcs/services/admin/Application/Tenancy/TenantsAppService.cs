using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Common.Authorization;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Admin.Application.Tenancy.Entities;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Admin.Application.Tenancy;

public class TenantsAppService(IServiceProvider services) : BaseAppService, ITenantsAppService
{
    private readonly TenantsManager _manager = services.GetRequiredService<TenantsManager>();
    private readonly TenantCreatingValidator _creatingValidator = services.GetRequiredService<TenantCreatingValidator>();
    private readonly TenantUpdatingValidator _updatingValidator = services.GetRequiredService<TenantUpdatingValidator>();

    [AppAuthorize(Constants.Permissions.Tenancy_Write)]
    public async Task<TenantDto> CreateAsync(TenantCreateDto request)
    {
        _creatingValidator.ValidateAndThrow(request);
        var entity = await _manager.CreateAsync(Map<TenantEntity>(request));
        return Map<TenantDto>(entity);
    }

    [AppAuthorize(Constants.Permissions.Tenancy_Write)]
    public Task DeleteAsync(int id)
    {
        return _manager.DeleteAsync(id);
    }

    [AppAuthorize(Constants.Permissions.Tenancy_Read)]
    public async Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request)
    {
        var total = await _manager.Queryable.CountAsync();
        var items = await _manager.Queryable
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync();

        return new PaginatedResult<TenantListDto>()
        {
            Total = total,
            Items = items.Adapt<List<TenantListDto>>(),
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    [AppAuthorize(Constants.Permissions.Tenancy_Read)]
    public async Task<TenantDto> GetAsync(int id)
    {
        var entity = await _manager.GetAsync(id);
        return Map<TenantDto>(entity);
    }

    [AppAuthorize(Constants.Permissions.Tenancy_Write)]
    public async Task<TenantDto> UpdateAsync(int id, TenantUpdateDto request)
    {
        _updatingValidator.ValidateAndThrow(request);
        var entity = await _manager.GetAsync(id);
        MapTo(request, entity);
        entity = await _manager.UpdateAsync(entity);
        return Map<TenantDto>(entity);
    }
}
