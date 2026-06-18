using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application.Authorization.Tenants;

public class TenantsManager(IServiceProvider services)
    : BaseManager<MicroserviceSrvIdentityDbContext>(services)
{
    public IQueryable<UserTenantEntity> Queryable => _dbContext.AppUserTenants;

    public async Task<List<UserTenantEntity>> GetByTenantAsync(int tenantId, int offset, int limit)
    {
        return await _dbContext.AppUserTenants
            .Where(ut => ut.TenantId == tenantId)
            .Include(ut => ut.User)
                .ThenInclude(u => u.Identities)
            .OrderBy(ut => ut.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> CountByTenantAsync(int tenantId)
    {
        return await _dbContext.AppUserTenants
            .Where(ut => ut.TenantId == tenantId)
            .CountAsync();
    }

    public async Task<List<long>> GetUserIdsByEmailsAsync(string[] emails)
    {
        return await _dbContext.AppIdentities
            .Where(i => emails.Contains(i.EmailAddress))
            .Select(i => i.UserId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<string>> GetEmailsByUserIdsAsync(long[] userIds)
    {
        return await _dbContext.AppIdentities
            .Where(i => userIds.Contains(i.UserId))
            .Select(i => i.EmailAddress)
            .ToListAsync();
    }

    public async Task AssignUsersAsync(int tenantId, IEnumerable<long> userIds)
    {
        var existingIds = await _dbContext.AppUserTenants
            .Where(ut => ut.TenantId == tenantId && userIds.Contains(ut.UserId))
            .Select(ut => ut.UserId)
            .ToListAsync();

        var newEntries = userIds
            .Except(existingIds)
            .Select(uid => new UserTenantEntity
            {
                UserId = uid,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow
            });

        _dbContext.AppUserTenants.AddRange(newEntries);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveUsersAsync(long tenantId, IEnumerable<long> userIds)
    {
        var entries = await _dbContext.AppUserTenants
            .Where(ut => ut.TenantId == tenantId && userIds.Contains(ut.UserId))
            .ToListAsync();

        _dbContext.AppUserTenants.RemoveRange(entries);
        await _dbContext.SaveChangesAsync();
    }
}
