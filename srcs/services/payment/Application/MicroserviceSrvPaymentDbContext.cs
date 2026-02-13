using datntdev.Microservice.Shared.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Payment.Application;

public class MicroserviceSrvPaymentDbContext(DbContextOptions<MicroserviceSrvPaymentDbContext> options)
    : BaseDbContext(options), IRelationalDbContext
{
}
