using datntdev.Microservice.Shared.Web.Host.Extensions;
using datntdev.Microservice.Shared.Web.Host.Hosting;

namespace datntdev.Microservice.Srv.Identity.Web.Host;

public class Startup : WebStartup<MicroserviceSrvIdentityWebHostModule>
{
    public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDefaultServices(configs);
        services.AddControllers();
    }

    public override void Configure(WebApplication app, IConfigurationRoot configs)
    {
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

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
