using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Identity.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Identity.Application;

[DependOn(typeof(MicroserviceSrvIdentityContractModule))]
public class MicroserviceSrvIdentityApplicationModule : BaseModule
{
    override public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceSrvIdentityDbContext>();
    }
}
