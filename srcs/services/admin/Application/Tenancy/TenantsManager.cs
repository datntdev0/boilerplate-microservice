using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Srv.Admin.Application.Tenancy.Entities;

namespace datntdev.Microservice.Srv.Admin.Application.Tenancy;

public class TenantsManager(IServiceProvider services)
    : BaseManager<int, TenantEntity, MicroserviceSrvAdminDbContext>(services)
{
    public override async Task<TenantEntity> CreateAsync(TenantEntity entity)
    {
        entity.Id = entity.Name.GetHashCode();
        var createdEntity = _dbContext.AppTenants.Add(entity);
        await _dbContext.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public override async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        _dbContext.AppTenants.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public override async Task<TenantEntity> GetAsync(int id)
    {
        var entity = await _dbContext.AppTenants.FindAsync(id);
        return entity is null ? throw new ExceptionNotFound() : entity;
    }

    public override async Task<TenantEntity> UpdateAsync(TenantEntity entity)
    {
        var updatedEntity = _dbContext.AppTenants.Update(entity);
        await _dbContext.SaveChangesAsync();
        return updatedEntity.Entity;
    }
}
