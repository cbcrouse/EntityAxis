using EntityAxis.Abstractions;
using MediatR;

namespace EntityAxis.MediatR.Queries;

/// <summary>
/// Represents a MediatR query for retrieving a single entity by its identifier.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public class GetEntityByIdQuery<TEntity, TKey> : IRequest<TEntity?>
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetEntityByIdQuery{TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    public GetEntityByIdQuery(TKey id)
    {
        Id = id;
    }

    /// <summary>
    /// The identifier of the entity to retrieve.
    /// </summary>
    public TKey Id { get; }
}