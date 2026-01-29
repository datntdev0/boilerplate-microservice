namespace datntdev.Microservice.Tests.Srv.Notify
{
    /// <summary>
    /// This is a required test class to ensure the "srv-notify" service is started in the Aspire Testing App Host.
    /// We should clone this pattern for each microservice to validate their startup within the test environment.
    /// </summary>
    [TestClass]
    public class MicroserviceSrvNotifyTests : MicroserviceSrvNotifyBaseTest
    {
        [AssemblyInitialize]
        public static Task AssemblyInitialize(TestContext testContext)
        {
            return StaticAssemblyInitialize(testContext);
        }

        [TestMethod]
        [DataRow("/alive")]
        [DataRow("/health")]
        public async Task GetHealthCheck_ReturnHealthy(string endpoint)
        {
            // Act
            using var response = await HttpClient.GetAsync(endpoint, CancellationToken);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Healthy", await response.Content.ReadAsStringAsync());
        }
    }
}
