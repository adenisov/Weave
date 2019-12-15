using System;
using System.Collections.Generic;

namespace Weave.Messaging.Core
{
    public static class CollectionsExtensions
    {
        /// <summary>
        /// Applies an action over the sequence by enumerating it.
        /// <remarks>Calls the enumerator!</remarks>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        [Obsolete("Will be moved to a shared library")]
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var i in source)
            {
                action(i);
            }
        }
    }
}
