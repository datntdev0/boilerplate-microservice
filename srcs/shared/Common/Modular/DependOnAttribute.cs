namespace datntdev.Microservice.Shared.Common.Modular;

/// <summary>
/// Used to define dependencies of module to other modules.
/// It should be used for a class derived from <see cref="BaseModule"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependOnAttribute(params Type[] dependingModules) : Attribute
{
    /// <summary>
    /// Types of depending modules.
    /// </summary>
    public Type[] DependingModules { get; } = dependingModules;
}