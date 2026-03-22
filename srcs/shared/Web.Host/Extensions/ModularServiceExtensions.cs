using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Shared.Web.Host.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Web.Host.Extensions;

public static class ModularServiceExtensions
{
    public static IServiceCollection AddServiceControllers(this IServiceCollection services, IEnumerable<BaseModule> modules)
    {
        var controllerFeatureProvider = new AppServiceFeatureProvider();
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                var existingAssemblies = manager.ApplicationParts.OfType<AssemblyPart>().Select(x => x.Assembly);
                var addingAssemblies = modules.Select(m => m.GetType().Assembly).Except(existingAssemblies).ToList();
                addingAssemblies.ForEach(assembly => manager.ApplicationParts.Add(new AssemblyPart(assembly)));
                manager.FeatureProviders.Add(controllerFeatureProvider);
            });
        services.Configure<MvcOptions>(options => options.Conventions.Add(controllerFeatureProvider));
        return services;
    }
}
