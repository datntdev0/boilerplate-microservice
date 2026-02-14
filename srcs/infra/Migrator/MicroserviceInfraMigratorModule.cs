using datntdev.Microservice.App.Identity;
using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Admin.Application;
using datntdev.Microservice.Srv.Identity.Application;
using datntdev.Microservice.Srv.Notify.Application;
using datntdev.Microservice.Srv.Payment.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Infra.Migrator;

[DependOn(
    typeof(MicroserviceAppIdentityModule),
    typeof(MicroserviceSrvAdminApplicationModule),
    typeof(MicroserviceSrvIdentityApplicationModule),
    typeof(MicroserviceSrvNotifyApplicationModule),
    typeof(MicroserviceSrvPaymentApplicationModule))]
internal class MicroserviceInfraMigratorModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddHostedService<MicroserviceInfraMigratorWorker>();

        var migrationsAssembly = GetType().Assembly.GetName().Name;

        services.ConfigureDbContext<MicroserviceAppIdentityDbContext>(
            opt => opt.UseSqlServer(configs.GetConnectionString("App.Identity"),
                o => o.MigrationsAssembly(migrationsAssembly)));
        services.ConfigureDbContext<MicroserviceSrvIdentityDbContext>(
            opt => opt.UseSqlServer(configs.GetConnectionString("Srv.Identity"),
                o => o.MigrationsAssembly(migrationsAssembly)));
        services.ConfigureDbContext<MicroserviceSrvPaymentDbContext>(
            opt => opt.UseSqlServer(configs.GetConnectionString("Srv.Payment"),
                o => o.MigrationsAssembly(migrationsAssembly)));
        services.ConfigureDbContext<MicroserviceSrvAdminDbContext>(
            opt => opt.UseMongoDB(configs.GetConnectionString("Srv.Admin")!));
        services.ConfigureDbContext<MicroserviceSrvNotifyDbContext>(
            opt => opt.UseMongoDB(configs.GetConnectionString("Srv.Notify")!));
    }
}
