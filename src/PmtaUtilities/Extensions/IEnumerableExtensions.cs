using System;
using System.Collections.Generic;

namespace PmtaUtilities.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (T element in source) act(element);
            return source;
        }
    }
}
