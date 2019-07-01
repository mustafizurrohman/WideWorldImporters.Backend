using System;
using System.Collections.Generic;
using System.Linq;
using WideWorldImporters.Core.Helpers;

namespace WideWorldImporters.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for IEnumerable
    /// </summary>
    public static class IEnumerableExtensions
    {


        /// <summary>
        /// Converts an IEnumerable to its CSV representation
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="ienumerableList">IEnumerableö of Type T to convert to CSV</param>
        /// <returns></returns>
        public static string ToCsv<T>(this IEnumerable<T> ienumerableList)
        {
            return ienumerableList.ToCsv();
        }

        /// <summary>
        /// Groups a IEnumerable w.r.t. an attribute
        /// </summary>
        /// <typeparam name="TSource">Type of source IEnumerable</typeparam>
        /// <typeparam name="TKey">Type of attribute of IEnumerable w.r.t. the IEnumerable must be grouped</typeparam>
        /// <param name="source">Source IEnumerable</param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source?.GroupBy(keySelector).Select(grp => grp.First());
        }

        /// <summary>
        /// Gets a random element for the IEnumerable
        /// </summary>
        /// <typeparam name="T">Type of source IEnumerable</typeparam>
        /// <param name="source">Source IEnumerable collection</param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            // If the list is empty then return an empty instance of T
            if (source.Count() == 0)
            {
                return Activator.CreateInstance<T>();
            }

            // Get a random number
            int randomNumber = IntHelpers.GetRandomNumber(source.Count() - 1);

            return source.ElementAt(randomNumber);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.Shuffle();

        /// <summary>
        /// Gets a random element for the IEnumerable after one shuffle
        /// </summary>
        /// <typeparam name="T">Type of source IEnumerable</typeparam>
        /// <param name="source">Source IEnumerable collection</param>
        /// <param name="shuffleTimes">Number of times to shuffle</param>
        /// <returns></returns>
        public static T GetRandomShuffled<T>(this IEnumerable<T> source, int shuffleTimes = 1)
        {
            shuffleTimes = (shuffleTimes < 1) ? 1 : shuffleTimes;

            for (int i = 0; i < shuffleTimes; i++)
            {
                source = source.Shuffle();
            }

            return source.GetRandomElement();
        }

        /// <summary>
        /// Returns 'true' if an IEnumerable is empty. False otherwise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return (list == null || !list.Any());
        }

    }
}
