using datntdev.Microservice.Tests.Common;
using Projects;

namespace datntdev.Microservice.Tests.Srv.Payment
{
    public class MicroserviceSrvPaymentBaseTest : MicroserviceBaseTest<datntdev_Microservice_Infra_Aspire>
    {
        public override HttpClient HttpClient => AppHost.CreateHttpClient("srv-payment");
    }
}
