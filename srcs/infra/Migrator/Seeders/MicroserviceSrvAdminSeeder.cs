using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Srv.Admin.Application;
using datntdev.Microservice.Srv.Admin.Application.Tenancy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Infra.Migrator.Seeders;

internal class MicroserviceSrvAdminSeeder(IServiceProvider services)
{
    private readonly MicroserviceSrvAdminDbContext _dbContext = services.GetRequiredService<MicroserviceSrvAdminDbContext>();

    public async Task SeedAsync()
    {
        await EnsureDefaultTenantExistsAsync();
    }

    private async Task EnsureDefaultTenantExistsAsync()
    {
        var existingTenant = await _dbContext.AppTenants.FirstOrDefaultAsync(t
            => t.Name == Constants.DefaultTenant.Name);
            
        if (existingTenant != null)
        {
            _dbContext.AppTenants.Remove(existingTenant);
            await _dbContext.SaveChangesAsync();
        }

        var newTenant = new TenantEntity
        {
            Name = Constants.DefaultTenant.Name,
            Organization = Constants.DefaultTenant.Organization
        };

        await _dbContext.AppTenants.AddAsync(newTenant);
        await _dbContext.SaveChangesAsync();
    }
}