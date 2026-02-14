using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Identity.Application;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.Srv.Identity.Web.Host;

[DependOn(typeof(MicroserviceSrvIdentityApplicationModule))]
public class MicroserviceSrvIdentityWebHostModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.ConfigureDbContext<MicroserviceSrvIdentityDbContext>(
            opt => opt.UseSqlServer(configs.GetConnectionString("Srv.Identity")));
    }
}
