using datntdev.Microservice.Shared.Application.Repository;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Roles.Entities;
using datntdev.Microservice.Srv.Identity.Application.Authorization.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Identity.Application;

public class MicroserviceSrvIdentityDbContext(DbContextOptions<MicroserviceSrvIdentityDbContext> options)
    : BaseDbContext(options), IRelationalDbContext
{
    public DbSet<UserEntity> AppUsers { get; set; }
    public DbSet<RoleEntity> AppRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
