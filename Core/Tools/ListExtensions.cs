using System.Collections.Generic;

namespace poetools.Core.Tools
{
    /// <summary>
    /// Extension methods for making it easier to work with System.Collections.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds a set of values to a collection.
        /// </summary>
        /// <param name="collection">The collection to add the values into.</param>
        /// <param name="values">The values to add into the collection.</param>
        /// <typeparam name="T">The type of data to be stored in the collection.</typeparam>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            foreach (var value in values)
                collection.Add(value);
        }
    }
}
