using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

public class IdentitiesManager(IServiceProvider services)
    : BaseManager<long, IdentityEntity, MicroserviceSrvIdentityDbContext>(services)
{
    private readonly PasswordHasher _passwordHasher = new();

    public override async Task<IdentityEntity> GetAsync(long id)
    {
        var entity = await _dbContext.AppIdentities.FindAsync(id);
        return entity is null ? throw new ExceptionNotFound() : entity;
    }

    public override async Task<IdentityEntity> CreateAsync(IdentityEntity entity)
    {
        entity = _passwordHasher.SetPassword(entity, entity.PasswordText);
        var createdEntity = _dbContext.AppIdentities.Add(entity);
        await _dbContext.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public override async Task<IdentityEntity> UpdateAsync(IdentityEntity entity)
    {
        entity = _passwordHasher.SetPassword(entity, entity.PasswordText);
        var updatedEntity = _dbContext.AppIdentities.Update(entity);
        await _dbContext.SaveChangesAsync();
        return updatedEntity.Entity;
    }

    public override async Task DeleteAsync(long id)
    {
        var entity = await GetAsync(id);
        _dbContext.AppIdentities.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
