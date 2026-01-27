using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Payment.Contracts;

namespace datntdev.Microservice.Srv.Payment.Application;

[DependOn(typeof(MicroserviceSrvPaymentContractModule))]
public class MicroserviceSrvPaymentApplicationModule : BaseModule
{
}
