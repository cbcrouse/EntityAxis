using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.Abstractions;

/// <summary>
/// Defines a command contract for updating an existing entity.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public interface IUpdate<TEntity, TKey> where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Update an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The updated entity identifier.</returns>
    Task<TKey> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}