using EntityAxis.Abstractions;
using MediatR;
using System.Collections.Generic;

namespace EntityAxis.MediatR.Queries;

/// <summary>
/// Represents a MediatR query for retrieving all entities of a given type.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public class GetAllEntitiesQuery<TEntity, TKey> : IRequest<List<TEntity>>
    where TEntity : IEntityId<TKey>
{
}
