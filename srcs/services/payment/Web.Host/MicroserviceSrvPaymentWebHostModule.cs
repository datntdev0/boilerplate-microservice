using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Payment.Application;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Payment.Web.Host;

[DependOn(typeof(MicroserviceSrvPaymentApplicationModule))]
public class MicroserviceSrvPaymentWebHostModule : BaseModule
{
    override public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.ConfigureDbContext<MicroserviceSrvPaymentDbContext>(
            opt => opt.UseSqlServer(configs.GetConnectionString("Srv.Payment")));
    }
}
