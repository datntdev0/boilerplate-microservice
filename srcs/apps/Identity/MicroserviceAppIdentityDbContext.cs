using datntdev.Microservice.Shared.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.App.Identity;

public class MicroserviceAppIdentityDbContext(DbContextOptions<MicroserviceAppIdentityDbContext> options)
    : BaseDbContext(options), IRelationalDbContext
{
}
