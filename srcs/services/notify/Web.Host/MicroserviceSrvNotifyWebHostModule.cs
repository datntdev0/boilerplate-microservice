using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Notify.Application;

namespace datntdev.Microservice.Srv.Notify.Web.Host;

[DependOn(typeof(MicroserviceSrvNotifyApplicationModule))]
public class MicroserviceSrvNotifyWebHostModule : BaseModule
{
}
