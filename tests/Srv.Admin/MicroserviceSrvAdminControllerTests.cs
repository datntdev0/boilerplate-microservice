using System.Net.Http.Json;

namespace datntdev.Microservice.Tests.Srv.Admin
{
    [TestClass]
    public class MicroserviceSrvAdminControllerTests : MicroserviceSrvAdminBaseTest
    {
        [TestMethod]
        public async Task GetWeatherForecast_ReturnOk()
        {
            // Act
            var client = await GetAuthenticatedClientAsync();
            using var response = await client.GetAsync("/WeatherForecast", CancellationToken);
            var content = await response.Content.ReadFromJsonAsync<List<object>>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotEmpty(content!);
        }
    }
}
