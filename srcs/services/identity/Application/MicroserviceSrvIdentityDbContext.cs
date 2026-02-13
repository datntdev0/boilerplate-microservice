using datntdev.Microservice.Shared.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Identity.Application;

public class MicroserviceSrvIdentityDbContext(DbContextOptions<MicroserviceSrvIdentityDbContext> options)
    : BaseDbContext(options), IRelationalDbContext
{
}
