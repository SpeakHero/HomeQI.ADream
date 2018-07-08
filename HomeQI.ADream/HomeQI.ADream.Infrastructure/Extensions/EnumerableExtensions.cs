using System.Linq;
using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {

        /// <summary>
        /// join ,
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> values)
        {
            return Join(values, ",");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> values, string split)
        {
            if (values == null)
            {
                return null;
            }
            var result = values.Aggregate(string.Empty, (current, value) => current + (split + value));
            result = result.TrimStart(split.ToCharArray());
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
            return source;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> key) where TKey : class
        {
            if (source == null)
            {
                return null;
            }
            var results = new List<T>();
            foreach (var item in source)
            {
                if (!results.Any(resultItem => key(resultItem) == key(item)))
                {
                    results.Add(item);
                }
            }
            return results;
        }

    }
}
