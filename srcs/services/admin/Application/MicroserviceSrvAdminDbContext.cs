using datntdev.Microservice.Shared.Application.Repository;
using datntdev.Microservice.Srv.Admin.Application.Tenancy.Entities;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Admin.Application;

public class MicroserviceSrvAdminDbContext(DbContextOptions<MicroserviceSrvAdminDbContext> options)
    : BaseDbContext(options), IDocumentDbContext
{
    public DbSet<TenantEntity> AppTenants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}
