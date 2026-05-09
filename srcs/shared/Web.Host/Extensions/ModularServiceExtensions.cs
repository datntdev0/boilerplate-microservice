using datntdev.Microservice.Shared.Application.Authorization;
using datntdev.Microservice.Shared.Application.Services;
using datntdev.Microservice.Shared.Common.Modular;
using datntdev.Microservice.Shared.Web.Host.Filters;
using datntdev.Microservice.Shared.Web.Host.Providers;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Web.Host.Extensions;

public static class ModularServiceExtensions
{
    public static IServiceCollection AddServiceControllers(this IServiceCollection services, IEnumerable<BaseModule> modules)
    {
        // Register PropertyInjectionFilter for automatic property injection
        services.AddScoped<PropertyInjectionFilter>();

        // Register default app services from all modules
        services.AddScoped<SessionAppProvider>();
        
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

    public static IServiceCollection AddDefaultAppServices(this IServiceCollection services, BaseModule module)
    {
        // Find the providers inheriting from BaseSingletonAppProvider and register them as singleton services
        var singletonProviderTypes = module.GetType().Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseSingletonAppProvider)));
        singletonProviderTypes.ToList().ForEach(type => services.AddSingleton(type));

        // Find the providers inheriting from BaseScopedAppProvider and register them as scoped services
        var scopedProviderTypes = module.GetType().Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseScopedAppProvider)));
        scopedProviderTypes.ToList().ForEach(type => services.AddScoped(type));

        // Find the managers inheriting from BaseManager and register them as scoped services
        var managerTypes = module.GetType().Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseManager)));
        managerTypes.ToList().ForEach(type => services.AddScoped(type));

        // Find the validator inheriting from IValidator and register them as scoped services
        var validatorTypes = module.GetType().Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IValidator)));
        validatorTypes.ToList().ForEach(type => services.AddScoped(type));

        return services;
    }

    public static IServiceCollection AddMapsterRegisters(this IServiceCollection services, BaseModule module)
    {
        TypeAdapterConfig.GlobalSettings.Scan(module.GetType().Assembly);
        return services;
    }
}
