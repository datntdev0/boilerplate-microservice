namespace datntdev.Microservice.Shared.Common.Application;

/// <summary>
/// Marks a property for automatic dependency injection before action execution.
/// Used by the PropertyInjectionFilter to inject services from IServiceProvider.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class AppInjectAttribute : Attribute
{
}
