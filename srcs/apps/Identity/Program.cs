using datntdev.Microservice.App.Identity.Components;
using datntdev.Microservice.Shared.Web.Host.Extensions;
using datntdev.Microservice.Shared.Web.Host.Hosting;

namespace datntdev.Microservice.App.Identity;

internal class Startup : WebStartup<MicroserviceAppIdentityModule>
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDefaultServices(configs);
        services.AddRazorComponents();
    }

    public override void Configure(WebApplication app, IConfigurationRoot configs)
    {
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();

        app.MapRazorComponents<Root>();

        app.MapDefaultHealthChecks();
    }
}

internal class Program
{
    public static void Main(string[] args)
    {
        new Startup().Build(args).Run();
    }
}