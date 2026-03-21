using datntdev.Microservice.Tests.Common;
using IdentityProgram = datntdev.Microservice.Srv.Identity.Web.Host.Program;

namespace datntdev.Microservice.Tests.Srv.Identity;

public class MicroserviceSrvIdentityBaseTest : MicroserviceBaseTest<IdentityProgram>
{
    public override HttpClient HttpClient => AppFactory.CreateClient();
}
