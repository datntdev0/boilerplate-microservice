using datntdev.Microservice.Tests.Common;
using Projects;

namespace datntdev.Microservice.Tests.Srv.Identity
{
    public class MicroserviceSrvIdentityBaseTest : MicroserviceBaseTest<datntdev_Microservice_Infra_Aspire>
    {
        public override HttpClient HttpClient => AppHost.CreateHttpClient("srv-identity");
    }
}
