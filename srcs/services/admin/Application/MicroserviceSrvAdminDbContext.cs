using datntdev.Microservice.Shared.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Admin.Application;

public class MicroserviceSrvAdminDbContext(DbContextOptions<MicroserviceSrvAdminDbContext> options)
    : BaseDbContext(options), IDocumentDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}
