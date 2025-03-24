using EntityAxis.Abstractions;

namespace EntityAxis.MediatR.Commands;

/// <summary>
/// Represents a command model intended for updating an entity.
/// </summary>
/// <typeparam name="TEntity">The entity type being updated.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public interface IUpdateCommandModel<TEntity, TKey> : IEntityId<TKey> {}
