using EntityAxis.Abstractions;
using MediatR;
using System;

namespace EntityAxis.MediatR.Commands;

/// <summary>
/// Represents a MediatR command for updating an existing entity.
/// </summary>
/// <typeparam name="TModel">The model used to update the entity.</typeparam>
/// <typeparam name="TEntity">The type of the domain entity being updated.</typeparam>
/// <typeparam name="TKey">The type of the identifier for the entity.</typeparam>
public class UpdateEntityCommand<TModel, TEntity, TKey> : IRequest<TKey>
    where TEntity : IEntityId<TKey>
    where TModel : IUpdateCommandModel<TEntity, TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEntityCommand{TModel, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="updateModel">The model used to update the entity.</param>
    public UpdateEntityCommand(TModel updateModel)
    {
        UpdateModel = updateModel ?? throw new ArgumentNullException(nameof(updateModel));
    }

    /// <summary>
    /// The model used to update the entity.
    /// </summary>
    public TModel UpdateModel { get; }
}
