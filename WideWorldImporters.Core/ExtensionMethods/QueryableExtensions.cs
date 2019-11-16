using System;
using System.Collections.Generic;
using System.Linq;

namespace WideWorldImporters.Core.ExtensionMethods
{

    /// <summary>
    /// Extension methods for IQueryable
    /// </summary>
    public static class QueryableExtensions
    {

        /// <summary>
        /// Simple method to chunk a source IQueryable into smaller (more manageable) lists
        /// </summary>
        /// <param name="source">The large IQueryable to split</param>
        /// <param name="chunkSize">The maximum number of items each subset should contain</param>
        /// <returns>An IEnumerable of the original source IEnumerable in bite size chunks</returns>
        public static IEnumerable<IEnumerable<TSource>> ChunkData<TSource>(this IQueryable<TSource> source, int chunkSize)
        {
            for (int i = 0; i < source.Count(); i += chunkSize)
                yield return source.Skip(i).Take(chunkSize);
        }

        /// <summary>
        /// Simple method to chunk a source IQueryable into smaller (more manageable) lists
        /// </summary>
        /// <param name="source">The large IQueryable to split</param>
        public static IEnumerable<IEnumerable<TSource>> ChunkData<TSource>(this IQueryable<TSource> source)
        {
            int chunkSize = (int)Math.Sqrt(source.Count());

            for (int i = 0; i < source.Count(); i += chunkSize)
                yield return source.Skip(i).Take(chunkSize);
        }

    }
}
