using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Admin.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Srv.Admin.Application;

[DependOn(typeof(MicroserviceSrvAdminContractModule))]
public class MicroserviceSrvAdminApplicationModule : BaseModule
{
    override public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceSrvAdminDbContext>();
    }
}
