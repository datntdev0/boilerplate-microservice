using datntdev.Microservice.Shared.Common.Application;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace datntdev.Microservice.Shared.Web.Host.Filters;

/// <summary>
/// Action filter that automatically injects dependencies into properties marked with [AppInject] attribute.
/// Runs before action execution to ensure properties are initialized.
/// </summary>
public class PropertyInjectionFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Inject properties on the controller instance before method execution
        var controller = context.Controller;
        InjectProperties(controller);

        // Continue with the action execution
        await next();
    }

    private void InjectProperties(object controller)
    {
        var controllerType = controller.GetType();

        // Get all properties with [AppInject] attribute
        var properties = controllerType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<AppInjectAttribute>() != null && p.CanWrite);

        foreach (var property in properties)
        {
            // Get the service from the service provider
            var service = _serviceProvider.GetRequiredService(property.PropertyType);

            // Inject the service into the property
            property.SetValue(controller, service);
        }
    }
}
