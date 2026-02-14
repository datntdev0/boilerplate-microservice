using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Notify.Application;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Notify.Web.Host;

[DependOn(typeof(MicroserviceSrvNotifyApplicationModule))]
public class MicroserviceSrvNotifyWebHostModule : BaseModule
{
    override public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.ConfigureDbContext<MicroserviceSrvNotifyDbContext>(
           opt => opt.UseMongoDB(configs.GetConnectionString("Srv.Notify")!));
    }
}
