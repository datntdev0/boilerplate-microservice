using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Model;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Identities.Dto;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

public class IdentitiesAppService(IServiceProvider services) : BaseAppService, IIdentitiesAppService
{
    private readonly IdentitiesManager _manager = services.GetRequiredService<IdentitiesManager>();
    private readonly IdentityCreatingValidator _creatingValidator = services.GetRequiredService<IdentityCreatingValidator>();
    private readonly IdentityUpdatingValidator _updatingValidator = services.GetRequiredService<IdentityUpdatingValidator>();

    public async Task<IdentityDto> CreateAsync(IdentityCreateDto request)
    {
        _creatingValidator.ValidateAndThrow(request);
        var entity = await _manager.CreateAsync(Map<IdentityEntity>(request));
        return Map<IdentityDto>(entity);
    }

    public Task DeleteAsync(long id)
    {
        return _manager.DeleteAsync(id);
    }

    public async Task<PaginatedResult<IdentityListDto>> GetAllAsync(PaginatedRequest request)
    {
        var total = await _manager.Queryable.CountAsync();
        var items = await _manager.Queryable
            .Skip(request.Offset)
            .Take(request.Limit)
            .ProjectToType<IdentityListDto>()
            .ToListAsync();
        return new PaginatedResult<IdentityListDto>()
        {
            Total = total,
            Items = items,
            Limit = request.Limit,
            Offset = request.Offset
        };
    }

    public async Task<IdentityDto> GetAsync(long id)
    {
        var entity = await _manager.GetAsync(id);
        return Map<IdentityDto>(entity);
    }

    public async Task<IdentityDto> UpdateAsync(long id, IdentityUpdateDto request)
    {
        _updatingValidator.ValidateAndThrow(request);
        var entity = await _manager.GetAsync(id);
        MapTo(request, entity);
        entity = await _manager.UpdateAsync(entity);
        return Map<IdentityDto>(entity);
    }
}
