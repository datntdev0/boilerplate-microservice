namespace datntdev.Microservice.Shared.Common.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">Enumerable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the enumerable</param>
    /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// Adds an item to a <see cref="ICollection{T}"/> if the given condition is true, and returns the collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection to add the item to</param>
    /// <param name="condition">A boolean value indicating whether to add the item</param>
    /// <param name="item">The item to add</param>
    /// <returns>The collection with the item added if the condition is true</returns>
    public static ICollection<T> AddIf<T>(this ICollection<T> collection, bool condition, T item)
    {
        if (condition) collection.Add(item); return collection;
    }
}