using System;
using System.Collections.Generic;

namespace EntityAxis.Abstractions
{
    /// <summary>
    /// Represents a paginated set of results for a query.
    /// </summary>
    /// <typeparam name="T">The type of entity returned in the result set.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// The items returned for the current page.
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// The total number of items across all pages.
        /// </summary>
        public int TotalItemCount { get; }

        /// <summary>
        /// The current page number (1-based).
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// The number of items requested per page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItemCount / PageSize);

        /// <summary>
        /// Creates a new paged result.
        /// </summary>
        public PagedResult(List<T> items, int totalItemCount, int pageNumber, int pageSize)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalItemCount = totalItemCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
