using datntdev.Microservice.Tests.Common;
using PaymentProgram = datntdev.Microservice.Srv.Payment.Web.Host.Program;

namespace datntdev.Microservice.Tests.Srv.Payment;

public class MicroserviceSrvPaymentBaseTest : MicroserviceBaseTest<PaymentProgram>
{
    public override HttpClient HttpClient => AppFactory.CreateClient();
}
