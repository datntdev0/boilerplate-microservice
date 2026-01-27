using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace datntdev.Microservice.Shared.Web.Host.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder Load(this IConfigurationBuilder configBuilder, IHostEnvironment env)
    {
        return configBuilder
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
            .AddJsonFile("appsettings.Common.json", optional: true)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddUserSecrets(Assembly.GetEntryAssembly()!, optional: true)
            .AddEnvironmentVariables();
    }
}
