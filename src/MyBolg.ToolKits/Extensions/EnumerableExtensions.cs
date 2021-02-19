using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBolg.ToolKits.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// 是否有重复
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="sources"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static bool HasRepeat<TSource, TKey>(this IEnumerable<TSource> sources, Func<TSource, TKey> keySelector)
        {
            sources.ThrowIfNull();
            var seenKeys = new HashSet<TKey>();
            return sources.Count(element => seenKeys.Add(keySelector(element))) != sources.Count();
        }

        /// <summary>
        /// 是否有重复
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static bool HasRepeat<TSource>(this IEnumerable<TSource> sources)
        {
            sources.ThrowIfNull();
            var seenKeys = new HashSet<TSource>();
            return sources.Count(item => seenKeys.Add(item)) != sources.Count();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<T> PageByIndex<T>(this IQueryable<T> query, int pageIndex = 1, int pageSize = 10)
        {
            query.ThrowIfNull();
            pageIndex = pageIndex >= 1 ? pageIndex : 1;
            pageSize = pageSize >= 10 ? pageSize : 10;
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 随机化IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, int count = -1)
        {
            source.ThrowIfNull();
            var rnd = new Random();
            source = source.OrderBy(item => rnd.Next());
            if (count > 0)
            {
                source = source.Take(count);
            }
            return source;
        }
    }
}