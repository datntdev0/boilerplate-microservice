using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Admin.Application;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Admin.Web.Host;

[DependOn(typeof(MicroserviceSrvAdminApplicationModule))]
public class MicroserviceSrvAdminWebHostModule : BaseModule
{
    override public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.ConfigureDbContext<MicroserviceSrvAdminDbContext>(
           opt => opt.UseMongoDB(configs.GetConnectionString("Srv.Admin")!));
    }
}
