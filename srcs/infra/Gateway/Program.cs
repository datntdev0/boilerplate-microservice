using datntdev.Microservice.Infra.Gateway.HealthChecks;
using datntdev.Microservice.Shared.Web.Host.Extensions;
using datntdev.Microservice.Shared.Web.Host.Hosting;

namespace datntdev.Microservice.Infra.Gateway;

public class Startup : WebStartup<MicroserviceInfraGatewayModule>
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        var yarpConfig = configs.GetSection("ReverseProxy");

        services.AddDefaultServices(configs);
        services.AddReverseProxy().LoadFromConfig(yarpConfig).AddServiceDiscoveryDestinationResolver();
        services.AddHealthChecks().AddCheck<GatewayHealthCheck>("gateway_health_check", tags: ["alive"]);
    }

    public override void Configure(WebApplication app, IConfigurationRoot configs)
    {
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapReverseProxy();

        app.MapDefaultHealthChecks();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        new Startup().Build(args).Run();
    }
}
