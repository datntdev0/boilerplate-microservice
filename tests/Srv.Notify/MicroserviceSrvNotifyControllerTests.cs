using System.Net.Http.Json;

namespace datntdev.Microservice.Tests.Srv.Notify
{
    [TestClass]
    public class MicroserviceSrvNotifyControllerTests : MicroserviceSrvNotifyBaseTest
    {
        [TestMethod]
        public async Task GetWeatherForecast_ReturnOk()
        {
            // Act
            using var response = await HttpClient.GetAsync("/WeatherForecast", CancellationToken);
            var content = await response.Content.ReadFromJsonAsync<List<object>>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotEmpty(content!);
        }
    }
}
