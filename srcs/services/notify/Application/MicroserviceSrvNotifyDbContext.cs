using datntdev.Microservice.Shared.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Notify.Application;

public class MicroserviceSrvNotifyDbContext(DbContextOptions<MicroserviceSrvNotifyDbContext> options)
    : BaseDbContext(options), IDocumentDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}
