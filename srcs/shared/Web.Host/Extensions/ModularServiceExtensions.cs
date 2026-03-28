using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Shared.Web.Host.Providers;
using FluentValidation;
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

    public static IServiceCollection AddDefaultServices(this IServiceCollection services, IEnumerable<BaseModule> modules)
    {
        // Find the managers inheriting from BaseManager and register them as scoped services
        var managerTypes = modules.SelectMany(m => m.GetType().Assembly.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseManager)));
        managerTypes.ToList().ForEach(type => services.AddScoped(type));

        // Find the validator inheriting from IValidator and register them as scoped services
        var validatorTypes = modules.SelectMany(m => m.GetType().Assembly.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IValidator)));
        validatorTypes.ToList().ForEach(type => services.AddScoped(type));

        return services;
    }
}
