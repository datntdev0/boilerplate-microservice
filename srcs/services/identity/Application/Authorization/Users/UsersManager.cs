using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Roles;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using datntdev.Microservice.Srv.Identity.Contracts.Authorization.Users.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Users;

public class UsersManager(IServiceProvider services)
    : BaseManager<long, UserEntity, MicroserviceSrvIdentityDbContext>(services)
{
    private readonly IdentitiesManager _identitiesManager = services.GetRequiredService<IdentitiesManager>();
    private readonly RolesManager _rolesManager = services.GetRequiredService<RolesManager>();

    public override async Task<UserEntity> GetAsync(long id)
    {
        var entity = await _dbContext.AppUsers
            .Where(x => x.Id == id)
            .Include(x => x.Roles)
            .Include(x => x.Identities)
            .FirstOrDefaultAsync();
        return entity is null ? throw new ExceptionNotFound() : entity;
    }

    public async Task<UserEntity> GetAsync(string emailAddress)
    {
        var entity = await _dbContext.AppUsers
            .Where(x => x.Identities.Any(i => i.EmailAddress == emailAddress))
            .Include(x => x.Roles)
            .Include(x => x.Identities)
            .Include(x => x.Tenants)
            .FirstOrDefaultAsync();
        return entity is null ? throw new ExceptionNotFound() : entity;
    }

    public override async Task<UserEntity> CreateAsync(UserEntity entity)
    {
        var createdEntity = _dbContext.AppUsers.Add(entity);
        await _dbContext.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public async Task<UserEntity> CreateAsync(UserCreateDto dto)
    {
        var createdUserEntity = await _identitiesManager.SignupAsync(
            email: dto.EmailAddress,
            password: dto.Password,
            firstName: dto.FirstName,
            lastName: dto.LastName);

        createdUserEntity.Permissions = dto.Permissions;
        createdUserEntity.Roles = await _rolesManager.GetByIdsAsync(dto.RoleIds);

        var updatedUserEntity = _dbContext.AppUsers.Update(createdUserEntity);
        await _dbContext.SaveChangesAsync();
        return updatedUserEntity.Entity;
    }

    public override async Task<UserEntity> UpdateAsync(UserEntity entity)
    {
        var updatedEntity = _dbContext.AppUsers.Update(entity);
        await _dbContext.SaveChangesAsync();
        return updatedEntity.Entity;
    }

    public async Task<UserEntity> UpdateAsync(UserUpdateDto dto)
    {
        var entity = await GetAsync(dto.Id);
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Permissions = dto.Permissions;
        entity.Roles = await _rolesManager.GetByIdsAsync(dto.RoleIds);

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
