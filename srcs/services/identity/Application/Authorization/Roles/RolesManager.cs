using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Roles.Entities;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Roles;

public class RolesManager(IServiceProvider services)
    : BaseManager<int, RoleEntity, MicroserviceSrvIdentityDbContext>(services)
{
    public override async Task<RoleEntity> GetAsync(int id)
    {
        var entity = await _dbContext.AppRoles.FindAsync(id);
        return entity is null ? throw new ExceptionNotFound() : entity;
    }

    public override async Task<RoleEntity> CreateAsync(RoleEntity entity)
    {
        var createdEntity = _dbContext.AppRoles.Add(entity);
        await _dbContext.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public override async Task<RoleEntity> UpdateAsync(RoleEntity entity)
    {
        var updatedEntity = _dbContext.AppRoles.Update(entity);
        await _dbContext.SaveChangesAsync();
        return updatedEntity.Entity;
    }

    public override async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        _dbContext.AppRoles.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
