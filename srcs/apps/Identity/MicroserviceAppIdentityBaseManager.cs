using datntdev.Microservice.Shared.Application.Services;

namespace datntdev.Microservice.App.Identity;

public class MicroserviceAppIdentityBaseManager(IServiceProvider services)
    : BaseManager<MicroserviceAppIdentityDbContext>(services)
{
}
