using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Common.Modular;

/// <summary>
/// Provides a base class for defining modular components in a microservices architecture.
/// Inherit from this class to implement your own module, and override <see cref="ConfigureServices"/>
/// to register services, and <see cref="Configure"/> to perform additional configuration.
/// </summary>
public abstract class BaseModule
{
    public virtual void ConfigureServices(IServiceCollection services, IConfigurationRoot configs) { }

    public virtual void Configure(IServiceProvider services, IConfigurationRoot configs) { }
}
