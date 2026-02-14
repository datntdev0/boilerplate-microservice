using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Payment.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Payment.Application;

[DependOn(typeof(MicroserviceSrvPaymentContractModule))]
public class MicroserviceSrvPaymentApplicationModule : BaseModule
{
    override public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceSrvPaymentDbContext>();
    }
}
