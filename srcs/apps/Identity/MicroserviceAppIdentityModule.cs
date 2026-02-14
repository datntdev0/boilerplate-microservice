using datntdev.Microservice.Shared.Common.Modular;

namespace datntdev.Microservice.App.Identity;

public class MicroserviceAppIdentityModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDbContext<MicroserviceAppIdentityDbContext>();
    }
}
