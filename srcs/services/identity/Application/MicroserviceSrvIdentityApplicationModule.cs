using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Identity.Contracts;

namespace datntdev.Microservice.Srv.Identity.Application;

[DependOn(typeof(MicroserviceSrvIdentityContractModule))]
public class MicroserviceSrvIdentityApplicationModule : BaseModule
{
}
