using System.Collections.Generic;
using UnityEngine;

namespace poetools.Core.Tools
{
    /// <summary>
    /// Utilities for formatting data into a human-readable form.
    /// </summary>
    public static class FormatTools
    {
        /// <summary>
        /// Returns a human-readable version of a list, formatted with commas.
        /// </summary>
        /// <param name="list">Data to format.</param>
        /// <typeparam name="T">Type of data to format.</typeparam>
        /// <returns>A readable string version of the input data.</returns>
        public static string PrettyList<T>(IList<T> list)
        {
            if (list.Count <= 0)
                return "None";
        
            string result = "";
        
            for (int i = 0; i < list.Count - 1; i++)
                result += $"{list[i].ToString()}, ";

            return result + list[list.Count - 1];
        }

        /// <summary>
        /// Determines whether you need an S or not when describing a list.
        /// </summary>
        /// <param name="source">The original string.</param>
        /// <param name="count">How many items are considered</param>
        /// <returns>The source, with possibly an "s" appended.</returns>
        public static string Plural(this string source, int count)
        {
            return source + (Mathf.Abs(count) != 1 ? "s" : string.Empty);
        }
    }
}