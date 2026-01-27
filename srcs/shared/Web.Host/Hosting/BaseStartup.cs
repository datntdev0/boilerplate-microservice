using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Shared.Web.Host.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace datntdev.Microservice.Shared.Web.Host.Hosting;

public interface IBaseStartup
{
    public abstract IHost Build(string[] args);
}

public class AppStartup<TBootstrapModule> : IBaseStartup
    where TBootstrapModule : BaseModule
{
    protected readonly List<BaseModule> _modules = [.. CreateAllModuleInstances()];

    public virtual IHost Build(string[] args)
    {
        var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(args);
        builder.Configuration.Load(builder.Environment);

        _modules.ForEach(module => module.ConfigureServices(builder.Services, builder.Configuration));
        var app = builder.Build();
        return app;
    }

    protected static IEnumerable<BaseModule> CreateAllModuleInstances()
    {
        return FindDependedModuleTypesRecursively(typeof(TBootstrapModule))
            .Append(typeof(TBootstrapModule))
            .Select(Activator.CreateInstance)
            .Select(module => (BaseModule)module!);
    }

    protected static IEnumerable<Type> FindDependedModuleTypesRecursively(Type moduleType)
    {
        if (!moduleType.GetTypeInfo().IsDefined(typeof(DependOnAttribute), true)) return [];

        var moduleTypes = moduleType.GetTypeInfo()
            .GetCustomAttributes(typeof(DependOnAttribute), true)
            .Cast<DependOnAttribute>()
            .SelectMany(x => x.DependingModules)
            .Distinct();

        return moduleTypes
            .SelectMany(FindDependedModuleTypesRecursively)
            .Concat(moduleTypes);
    }
}

public class WebStartup<TBootstrapModule> : AppStartup<TBootstrapModule>
    where TBootstrapModule : BaseModule
{
    public override IHost Build(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.Load(builder.Environment);

        _modules.ForEach(module => module.ConfigureServices(builder.Services, builder.Configuration));
        this.ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();

        _modules.ForEach(module => module.Configure(app.Services, builder.Configuration));
        this.Configure(app, builder.Configuration);
        return app;
    }

    public virtual void ConfigureServices(IServiceCollection services, IConfigurationRoot configs) { }

    public virtual void Configure(WebApplication app, IConfigurationRoot configs) { }
}