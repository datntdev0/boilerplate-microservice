using datntdev.Microservice.Shared.Web.Host.Hosting;
using Microsoft.Extensions.Hosting;

namespace datntdev.Microservice.Infra.Migrator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new AppStartup<MicroserviceInfraMigratorModule>().Build(args).Run();
        }
    }
}
