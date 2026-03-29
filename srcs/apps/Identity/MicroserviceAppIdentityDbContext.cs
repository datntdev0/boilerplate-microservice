using datntdev.Microservice.App.Identity.Models;
using datntdev.Microservice.Shared.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.App.Identity;

public class MicroserviceAppIdentityDbContext(DbContextOptions<MicroserviceAppIdentityDbContext> options)
    : BaseDbContext(options), IRelationalDbContext
{
    public DbSet<IdentityEntity> AppIdentities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityEntity>(entity =>
        {
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.EmailAddress);
            entity.Ignore(e => e.PasswordText);
        });
    }
}
