using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace datntdev.Microservice.Infra.Migrator;

internal class MicroserviceInfraMigrationWorker(IServiceProvider services) : IHostedLifecycleService
{
    private readonly ILogger<MicroserviceInfraMigrationWorker> _logger = services
        .GetRequiredService<ILogger<MicroserviceInfraMigrationWorker>>();
    private readonly IHostApplicationLifetime _lifetime = services
        .GetRequiredService<IHostApplicationLifetime>();

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        _ = Task.Delay(1000).ContinueWith((x) => MigrateAsync());
        return Task.CompletedTask;
    }

    private async Task MigrateAsync()
    {
        try
        {
            _logger.LogInformation("Migration started.");
            await Task.Delay(2000);
            _logger.LogInformation("Migration completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Migration failed with message: {Message}", ex.Message);
        }
        finally
        {
            _lifetime.StopApplication(); 
        }
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
