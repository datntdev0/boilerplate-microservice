using datntdev.Microservice.Tests.Common;
using Projects;

namespace datntdev.Microservice.Tests.Srv.Notify
{
    public class MicroserviceSrvNotifyBaseTest : MicroserviceBaseTest<datntdev_Microservice_Infra_Aspire>
    {
        public override HttpClient HttpClient => AppHost.CreateHttpClient("srv-notify");
    }
}
