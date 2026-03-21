using datntdev.Microservice.Tests.Common;
using NotifyProgram = datntdev.Microservice.Srv.Notify.Web.Host.Program;

namespace datntdev.Microservice.Tests.Srv.Notify;

public class MicroserviceSrvNotifyBaseTest : MicroserviceBaseTest<NotifyProgram>
{
    public override HttpClient HttpClient => AppFactory.CreateClient();
}
