using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Identities.Entities;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Identities;

public class IdentitiesManager(IServiceProvider services)
    : BaseManager<long, IdentityEntity, MicroserviceSrvIdentityDbContext>(services)
{
    private readonly PasswordHasher _passwordHasher = new();

    public override async Task<IdentityEntity> GetAsync(long id)
    {
        var entity = await _dbContext.AppIdentities
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
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

    public async Task<UserEntity> SigninAsync(string email, string password)
    {
        var identity = await _dbContext.AppIdentities
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.EmailAddress == email);

        if (identity == null)
            throw new ValidationException([new ValidationFailure(nameof(email), "Invalid credentials.")]);

        var result = _passwordHasher.VerifyHashedPassword(identity, identity.PasswordHash, password);
        if (result != PasswordVerificationResult.Success)
            throw new ValidationException([new ValidationFailure(nameof(password), "Invalid credentials.")]);

        return identity.User;
    }

    public async Task<UserEntity> SignupAsync(string email, string password, string firstName, string lastName)
    {
        var exists = await _dbContext.AppIdentities.AnyAsync(x => x.EmailAddress == email);
        if (exists)
            throw new ValidationException([new ValidationFailure(nameof(email), $"Email '{email}' is already registered.")]);

        var user = new UserEntity { FirstName = firstName, LastName = lastName };
        _dbContext.AppUsers.Add(user);
        await _dbContext.SaveChangesAsync();

        var identity = new IdentityEntity { EmailAddress = email, PasswordText = password, UserId = user.Id };
        identity = _passwordHasher.SetPassword(identity, password);
        _dbContext.AppIdentities.Add(identity);
        await _dbContext.SaveChangesAsync();

        return user;
    }
}
