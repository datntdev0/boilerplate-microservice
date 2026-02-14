using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Notify.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Notify.Application;

[DependOn(typeof(MicroserviceSrvNotifyContractModule))]
public class MicroserviceSrvNotifyApplicationModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceSrvNotifyDbContext>();
    }
}
