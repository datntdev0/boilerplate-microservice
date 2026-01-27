using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Srv.Admin.Contracts;

namespace datntdev.Microservice.Srv.Admin.Application;

[DependOn(typeof(MicroserviceSrvAdminContractModule))]
public class MicroserviceSrvAdminApplicationModule : BaseModule
{
}
