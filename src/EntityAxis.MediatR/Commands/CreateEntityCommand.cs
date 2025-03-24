using AutoMapper;
using EntityAxis.Abstractions;
using MediatR;
using System;

namespace EntityAxis.MediatR.Commands;

/// <summary>
/// Represents a MediatR command for creating a new entity.
/// </summary>
/// <typeparam name="TModel">The model used to create the entity.</typeparam>
/// <typeparam name="TEntity">The type of the domain entity to create.</typeparam>
/// <typeparam name="TKey">The type of the identifier for the entity.</typeparam>
/// <remarks>
/// An AutoMapper mapping must be explicitly configured from <typeparamref name="TModel"/> to <typeparamref name="TEntity"/>.
/// The corresponding handler relies on <see cref="IMapper"/>.Map&lt;TDestination&gt;(object) to transform the model into a <typeparamref name="TEntity"/>.
/// Failure to configure the mapping will result in a runtime <see cref="AutoMapperMappingException"/>.
/// </remarks>
public class CreateEntityCommand<TModel, TEntity, TKey> : IRequest<TKey>
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEntityCommand{TModel, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="createModel">The model used to create the entity.</param>
    public CreateEntityCommand(TModel createModel)
    {
        CreateModel = createModel ?? throw new ArgumentNullException(nameof(createModel));
    }

    /// <summary>
    /// The model used to create the entity.
    /// </summary>
    public TModel CreateModel { get; }
}