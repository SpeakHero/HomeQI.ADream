using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class AsyncPagedListExtensions
    {
        /// <summary>
        /// 异步创建对象集合的子集，该子集可以由索引单独访问，并包含关于子集创建的对象集合的元数据。
        /// </summary>
        /// <typeparam name="T">集合应该包含的对象类型。</typeparam>
        /// <param name="superset">T将对象划分为子集的集合。如果集合实现<seecre="IQueryable{t}"/>，则将被视为这样。</param>
        /// <returns>该对象集合的子集，可以通过索引单独访问，并包含关于对象集合的子集，该子集是从该对象创建的。</returns>
        /// <seealso cref="PagedList{T}"/>
        public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset)
        {
            return Task.Factory.StartNew(superset.ToList);
        }

        /// <summary>
        /// 创建可由索引单独访问的对象集合的子集，并包含有关从该子集创建的对象集合的元数据。
        /// </summary>
        /// <typeparam name="T">集合应该包含的对象类型。</typeparam>
        /// <typeparam name="TKey">比较类型</typeparam>
        /// <param name="superset">T将对象划分为子集的集合。如果集合实现<seecre="IQueryable{t}"/>，则将被视为这样。</param>
        /// <param name="pageNumber">这个实例包含的对象子集的一个索引。</param>
        /// <param name="pageSize">任何单个子集的最大大小。</param>
        /// <returns>该对象集合的子集，可以通过索引单独访问，并包含关于对象集合的子集，该子集是从该对象创建的。</returns>
        /// <seealso cref="PagedList{T}"/>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> list, int pageNumber, int pageSize)
        {
            return Task.Factory.StartNew(() => (IPagedList<T>)(new StaticPagedList<T>(list.Skip(((pageNumber - 1) * pageSize)).Take(pageSize), pageNumber, pageSize, list.Count())));
        }

        /// <summary>
        /// 异步创建对象集合的子集，该子集可以由索引单独访问，并包含关于子集创建的对象集合的元数据。
        /// </summary>
        /// <typeparam name="T">集合应该包含的对象类型。</typeparam>
        /// <typeparam name="TKey">比较类型</typeparam>
        /// <param name="superset">将对象划分为子集的集合。如果集合实现<seecre="IQueryable{t}"/>，则将被视为这样。</param>
        /// <param name="pageNumber">这个实例包含的对象子集的一个索引。</param>
        /// <param name="pageSize">任何单个子集的最大大小。</param>
        /// <returns>该对象集合的子集，可以通过索引单独访问，并包含关于对象集合的子集，该子集是从该对象创建的。</returns>
        /// <seealso cref="PagedList{T}"/>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return AsyncPagedList<T>.CreateAsync(superset, pageNumber, pageSize);
        }
    }
}
