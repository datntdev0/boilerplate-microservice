namespace datntdev.Microservice.Shared.Common.Application;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class AppRouteAttribute(string template) : Attribute
{
    public string Template { get; } = template;
}
