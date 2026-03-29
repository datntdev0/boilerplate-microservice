using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users;

public class UsersManager(IServiceProvider services)
    : BaseManager<long, UserEntity, MicroserviceSrvIdentityDbContext>(services)
{
    public override async Task<UserEntity> GetAsync(long id)
    {
        var entity = await _dbContext.AppUsers.FindAsync(id);
        return entity is null ? throw new ExceptionNotFound() : entity;
    }

    public override async Task<UserEntity> CreateAsync(UserEntity entity)
    {
        var createdEntity = _dbContext.AppUsers.Add(entity);
        await _dbContext.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public override async Task<UserEntity> UpdateAsync(UserEntity entity)
    {
        var updatedEntity = _dbContext.AppUsers.Update(entity);
        await _dbContext.SaveChangesAsync();
        return updatedEntity.Entity;
    }

    public override async Task DeleteAsync(long id)
    {
        var entity = await GetAsync(id);
        _dbContext.AppUsers.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
