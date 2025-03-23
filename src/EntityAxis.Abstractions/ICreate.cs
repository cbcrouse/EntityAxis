using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.Abstractions
{
    /// <summary>
    /// Defines a command contract for creating a new entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public interface ICreate<TEntity, TKey> where TEntity : IEntityId<TKey>
    {
        /// <summary>
        /// Create a new entity.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The created entity, typically with an assigned identifier.</returns>
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
