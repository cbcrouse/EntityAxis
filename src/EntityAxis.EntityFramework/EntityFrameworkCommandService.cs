using AutoMapper;
using EntityAxis.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EntityAxis.EntityFramework;

/// <summary>
/// Default EF Core implementation of <see cref="ICommandService{TEntity, TKey}"/>.
/// </summary>
/// <typeparam name="TEntity">The application entity.</typeparam>
/// <typeparam name="TDbEntity">The database entity mapped via AutoMapper.</typeparam>
/// <typeparam name="TDbContext">The EF Core DbContext.</typeparam>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public class EntityFrameworkCommandService<TEntity, TDbEntity, TDbContext, TKey> : EntityFrameworkServiceBase<TDbContext>, ICommandService<TEntity, TKey>
    where TEntity : class, IEntityId<TKey>
    where TDbEntity : class, IEntityId<TKey>
    where TDbContext : DbContext
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkCommandService{TEntity, TDbEntity, TDbContext, TKey}"/> class.
    /// </summary>
    /// <param name="contextFactory">Factory for creating DbContext instances.</param>
    /// <param name="mapper">AutoMapper instance for mapping entities.</param>
    public EntityFrameworkCommandService(IDbContextFactory<TDbContext> contextFactory, IMapper mapper)
        : base(contextFactory, mapper)
    {
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbEntity = _mapper.Map<TDbEntity>(entity);
        context.Set<TDbEntity>().Add(dbEntity);
        await context.SaveChangesAsync(cancellationToken);
        return dbEntity.Id;
    }

    /// <inheritdoc/>
    public async Task<TKey> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbSet = context.Set<TDbEntity>();

        TDbEntity? existing = await dbSet.FindAsync(keyValues:[entity.Id!], cancellationToken);
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

        TDbEntity? existing = await dbSet.FindAsync(keyValues:[id!], cancellationToken);
        if (existing == null)
        {
            return;
        }

        dbSet.Remove(existing);
        await context.SaveChangesAsync(cancellationToken);
    }
}
