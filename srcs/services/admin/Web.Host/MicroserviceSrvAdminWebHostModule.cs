using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Admin.Application;

namespace datntdev.Microservice.Srv.Admin.Web.Host;

[DependOn(typeof(MicroserviceSrvAdminApplicationModule))]
public class MicroserviceSrvAdminWebHostModule : BaseModule
{
}
