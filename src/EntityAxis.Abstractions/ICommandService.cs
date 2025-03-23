namespace EntityAxis.Abstractions
{
    /// <summary>
    /// Composes all entity mutation operations into a single command service interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public interface ICommandService<TEntity, TKey> :
        ICreate<TEntity, TKey>,
        IUpdate<TEntity, TKey>,
        IDelete<TEntity, TKey>
        where TEntity : IEntityId<TKey>
    {
    }
}
