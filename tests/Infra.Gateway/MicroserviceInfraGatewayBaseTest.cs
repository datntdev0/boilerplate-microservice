using datntdev.Microservice.Tests.Common;
using Projects;

namespace datntdev.Microservice.Tests.Infra.Gateway
{
    public class MicroserviceInfraGatewayBaseTest : MicroserviceBaseTest<datntdev_Microservice_Infra_Aspire>
    {
        public override HttpClient HttpClient => AppHost.CreateHttpClient("gateway");
    }
}
