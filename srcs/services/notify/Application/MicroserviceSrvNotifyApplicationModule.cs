using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Notify.Contracts;

namespace datntdev.Microservice.Srv.Notify.Application;

[DependOn(typeof(MicroserviceSrvNotifyContractModule))]
public class MicroserviceSrvNotifyApplicationModule : BaseModule
{
}
