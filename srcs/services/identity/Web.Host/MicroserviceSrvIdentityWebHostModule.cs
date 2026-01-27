using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Identity.Application;

namespace datntdev.Microservice.Srv.Identity.Web.Host;

[DependOn(typeof(MicroserviceSrvIdentityApplicationModule))]
public class MicroserviceSrvIdentityWebHostModule : BaseModule
{
}
