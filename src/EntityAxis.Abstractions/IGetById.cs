using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.Abstractions;

/// <summary>
/// Defines a query contract for retrieving a single entity by its identifier.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public interface IGetById<TEntity, TKey> where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Retrieve a single entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The entity, or null if not found.</returns>
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
}