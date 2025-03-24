namespace EntityAxis.Abstractions;

/// <summary>
/// Composes all entity query operations into a single service interface.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public interface IQueryService<TEntity, TKey> :
    IGetAll<TEntity, TKey>,
    IGetById<TEntity, TKey>,
    IGetPaged<TEntity, TKey>
    where TEntity : IEntityId<TKey>
{
}