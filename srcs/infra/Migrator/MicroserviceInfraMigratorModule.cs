using datntdev.Microservice.Shared.Common.Modular;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Infra.Migrator;

internal class MicroserviceInfraMigratorModule : BaseModule
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddHostedService<MicroserviceInfraMigrationWorker>();
    }
}
