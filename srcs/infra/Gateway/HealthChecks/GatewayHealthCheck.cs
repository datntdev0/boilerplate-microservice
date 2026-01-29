using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace datntdev.Microservice.Infra.Gateway.HealthChecks;

public class GatewayHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var clusters = _configuration.GetSection("ReverseProxy:Clusters").GetChildren();
        var clients = clusters.Select(x => _httpClientFactory.CreateClient(x.Key));
        var resultTasks = clients.Select(client => client.GetStringAsync("/alive", cancellationToken));

        var responses = await Task.WhenAll(resultTasks);

        return responses.All(r => r == "Healthy")
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy("One or more downstream services are unhealthy.");
    }
}