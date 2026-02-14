using datntdev.Microservice.App.Identity;
using datntdev.Microservice.Shared.Application.Repository;
using datntdev.Microservice.Srv.Admin.Application;
using datntdev.Microservice.Srv.Identity.Application;
using datntdev.Microservice.Srv.Notify.Application;
using datntdev.Microservice.Srv.Payment.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace datntdev.Microservice.Infra.Migrator;

internal class MicroserviceInfraMigratorWorker(IServiceProvider services) : IHostedLifecycleService
{
    private readonly IServiceProvider _services = services;

    private readonly ILogger<MicroserviceInfraMigratorWorker> _logger = services
        .GetRequiredService<ILogger<MicroserviceInfraMigratorWorker>>();
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
            await Task.WhenAll(
                StartMigrationAsync(_services.GetRequiredService<MicroserviceAppIdentityDbContext>()),
                StartMigrationAsync(_services.GetRequiredService<MicroserviceSrvIdentityDbContext>()),
                StartMigrationAsync(_services.GetRequiredService<MicroserviceSrvPaymentDbContext>()),
                StartMigrationAsync(_services.GetRequiredService<MicroserviceSrvAdminDbContext>()),
                StartMigrationAsync(_services.GetRequiredService<MicroserviceSrvNotifyDbContext>())
            );
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

    private Task StartMigrationAsync<TDbContext>(TDbContext dbContext)
        where TDbContext : DbContext
    {
        var logger = _services.GetRequiredService<ILogger<TDbContext>>();
        logger.LogInformation("Checking database existed or pending migrations...");

        // For MongoDb, Migrate is not supported,
        // so we only check pending migrations for relational databases.
        if (dbContext is IRelationalDbContext)
        {
            var pendingChanges = dbContext.Database.GetPendingMigrations();
            if (pendingChanges.Any()) dbContext.Database.Migrate();
        }

        // For MongoDB, EnsureCreated only creates the database if it contains collections.
        // If there are no collections, you won't see the database in the MongoDb server.
        if (dbContext is IDocumentDbContext)
        {
            dbContext.Database.EnsureCreated();
        }

        return Task.CompletedTask;
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
