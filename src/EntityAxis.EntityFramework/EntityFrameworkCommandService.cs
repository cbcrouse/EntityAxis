using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.KeyMappers;
using Microsoft.EntityFrameworkCore;

namespace EntityAxis.EntityFramework;

/// <summary>
/// Default EF Core implementation of <see cref="ICommandService{TEntity, TKey}"/>.
/// </summary>
/// <typeparam name="TEntity">The application entity.</typeparam>
/// <typeparam name="TDbEntity">The database entity mapped via AutoMapper.</typeparam>
/// <typeparam name="TDbContext">The EF Core DbContext.</typeparam>
/// <typeparam name="TKey">The type of the application entity's key.</typeparam>
/// <typeparam name="TDbKey">The type of the database entity's key.</typeparam>
public class EntityFrameworkCommandService<TEntity, TDbEntity, TDbContext, TKey, TDbKey> : EntityFrameworkServiceBase<TDbContext>, ICommandService<TEntity, TKey>
    where TEntity : class, IEntityId<TKey>
    where TDbEntity : class, IEntityId<TDbKey>
    where TDbContext : DbContext
{
    private readonly IMapper _mapper;
    private readonly IKeyMapper<TKey, TDbKey> _keyMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkCommandService{TEntity, TDbEntity, TDbContext, TKey, TDbKey}"/> class.
    /// </summary>
    /// <param name="contextFactory">Factory for creating DbContext instances.</param>
    /// <param name="mapper">AutoMapper instance for mapping entities.</param>
    /// <param name="keyMapper">Mapper for converting between application and database key types.</param>
    public EntityFrameworkCommandService(IDbContextFactory<TDbContext> contextFactory, IMapper mapper, IKeyMapper<TKey, TDbKey> keyMapper)
        : base(contextFactory, mapper)
    {
        _mapper = mapper;
        _keyMapper = keyMapper;
    }

    /// <inheritdoc/>
    public async Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbEntity = _mapper.Map<TDbEntity>(entity);
        context.Set<TDbEntity>().Add(dbEntity);
        await context.SaveChangesAsync(cancellationToken);
        return _keyMapper.ToAppKey(dbEntity.Id);
    }

    /// <inheritdoc/>
    public async Task<TKey> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbSet = context.Set<TDbEntity>();

        TDbKey dbKey = _keyMapper.ToDbKey(entity.Id);
        if (dbKey is null)
        {
            throw new InvalidOperationException("The entity's key cannot be null.");
        }

        TDbEntity? existing = await dbSet.FindAsync(keyValues: new object[] { dbKey }, cancellationToken);
        if (existing == null)
        {
            throw new InvalidOperationException($"{typeof(TDbEntity).Name} with id '{entity.Id}' was not found.");
        }

        _mapper.Map(entity, existing);
        dbSet.Update(existing);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbSet = context.Set<TDbEntity>();

        TDbKey dbKey = _keyMapper.ToDbKey(id);
        if (dbKey is null)
        {
            throw new InvalidOperationException("The entity's key cannot be null.");
        }

        TDbEntity? existing = await dbSet.FindAsync(keyValues: new object[] { dbKey }, cancellationToken);
        if (existing == null)
        {
            return;
        }

        dbSet.Remove(existing);
        await context.SaveChangesAsync(cancellationToken);
    }
}
