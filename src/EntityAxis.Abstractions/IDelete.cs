using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.Abstractions;

/// <summary>
/// Defines a command contract for deleting an entity by its identifier.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being deleted.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public interface IDelete<TEntity, in TKey> where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Delete the entity with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}