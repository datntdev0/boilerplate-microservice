using datntdev.Microservice.Tests.Common;
using AdminProgram = datntdev.Microservice.Srv.Admin.Web.Host.Program;

namespace datntdev.Microservice.Tests.Srv.Admin;

public class MicroserviceSrvAdminBaseTest : MicroserviceBaseTest<AdminProgram>
{
    public override HttpClient HttpClient => AppFactory.CreateClient();
}
