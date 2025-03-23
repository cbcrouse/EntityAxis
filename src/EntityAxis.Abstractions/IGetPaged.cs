using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.Abstractions
{
    /// <summary>
    /// Defines a query contract for retrieving entities using pagination.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public interface IGetPaged<TEntity, TKey> where TEntity : IEntityId<TKey>
    {
        /// <summary>
        /// Retrieve a page of entities.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A paged result of entities within the requested page.</returns>
        Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
