using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <remarks>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </remarks>
    /// <typeparam name="T">集合应该包含的对象类型。</typeparam>
    /// <seealso cref="IPagedList{T}"/>
    /// <seealso cref="BasePagedList{T}"/>
    /// <seealso cref="PagedList{T}"/>
    /// <seealso cref="List{T}"/>
    //[Serializable]
    public class StaticPagedList<T> : BasePagedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticPagedList{T}"/> class that contains the already divided subset and information about the size of the superset and the subset's position within it.
        /// </summary>
        /// <param name="subset">The single subset this collection should represent.</param>
        /// <param name="metaData">Supply the ".MetaData" property of an existing IPagedList instance to recreate it here (such as when creating a new instance of a PagedList after having used Automapper to convert its contents to a DTO.)</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public StaticPagedList(IEnumerable<T> subset, IPagedList metaData)
            : this(subset, metaData.PageNumber, metaData.PageSize, metaData.TotalItemCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticPagedList{T}"/> class that contains the already divided subset and information about the size of the superset and the subset's position within it.
        /// </summary>
        /// <param name="subset">The single subset this collection should represent.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects contained by this instance.</param>
        /// <param name="pageSize">任何单个子集的最大大小。</param>
        /// <param name="totalItemCount">The size of the superset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public StaticPagedList(IEnumerable<T> subset, int pageNumber, int pageSize, int totalItemCount)
            : base(pageNumber, pageSize, totalItemCount)
        {
            Subset.AddRange(subset);
        }
    }
}