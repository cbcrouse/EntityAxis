using EntityAxis.Abstractions;
using MediatR;

namespace EntityAxis.MediatR.Commands;

/// <summary>
/// Represents a MediatR command for deleting an entity by its identifier.
/// </summary>
/// <typeparam name="TEntity">The type of the domain entity to delete.</typeparam>
/// <typeparam name="TKey">The type of the identifier for the entity.</typeparam>
public class DeleteEntityCommand<TEntity, TKey> : IRequest
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteEntityCommand{TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    public DeleteEntityCommand(TKey id)
    {
        Id = id;
    }

    /// <summary>
    /// The identifier of the entity to delete.
    /// </summary>
    public TKey Id { get; }
}
