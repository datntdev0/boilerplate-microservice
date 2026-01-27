using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Payment.Application;

namespace datntdev.Microservice.Srv.Payment.Web.Host;

[DependOn(typeof(MicroserviceSrvPaymentApplicationModule))]
public class MicroserviceSrvPaymentWebHostModule : BaseModule
{
}
