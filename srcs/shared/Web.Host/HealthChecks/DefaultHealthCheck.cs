using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace datntdev.Microservice.Shared.Web.Host.HealthChecks;

internal class DefaultHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}