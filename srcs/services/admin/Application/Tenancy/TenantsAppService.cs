using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy;
using datntdev.Microservice.Srv.Admin.Contracts.Tenancy.Dto;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Admin.Application.Tenancy;

public class TenantsAppService(IServiceProvider services) : BaseAppService, ITenantsAppService
{
    private readonly TenantsManager _manager = services.GetRequiredService<TenantsManager>();
    private readonly TenantCreatingValidator _creatingValidator = services.GetRequiredService<TenantCreatingValidator>();
    private readonly TenantUpdatingValidator _updatingValidator = services.GetRequiredService<TenantUpdatingValidator>();

    public async Task<TenantDto> CreateAsync(TenantCreateDto request)
    {
        _creatingValidator.ValidateAndThrow(request);

        var entity = await _manager.CreateAsync(new()
        {
            Name = request.Name,
            Organization = request.Organization
        });
        return new TenantDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Organization = entity.Organization,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public Task DeleteAsync(int id)
    {
        return _manager.DeleteAsync(id);
    }

    public async Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request)
    {
        var total = await _manager.Queryable.CountAsync();
        var items = await _manager.Queryable
            .Skip(request.Offset)
            .Take(request.Limit)
            .Select(x => new TenantListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Organization = x.Organization
            })
            .ToListAsync();
        return new PaginatedResult<TenantListDto>()
        {
            Total = total,
            Items = items,
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    public async Task<TenantDto> GetAsync(int id)
    {
        var entity = await _manager.GetAsync(id);
        return new TenantDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Organization = entity.Organization,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public async Task<TenantDto> UpdateAsync(int id, TenantUpdateDto request)
    {
        _updatingValidator.ValidateAndThrow(request);

        var entity = await _manager.GetAsync(id);
        entity.Name = request.Name;
        entity.Organization = request.Organization;

        entity = await _manager.UpdateAsync(entity);
        return new TenantDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Organization = entity.Organization,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
