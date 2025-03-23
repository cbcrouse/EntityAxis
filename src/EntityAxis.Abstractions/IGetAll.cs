using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.Abstractions
{
    /// <summary>
    /// Defines a query contract for retrieving all entities of a given type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public interface IGetAll<TEntity, TKey> where TEntity : IEntityId<TKey>
    {
        /// <summary>
        /// Retrieve all entities.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of all entities.</returns>
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
