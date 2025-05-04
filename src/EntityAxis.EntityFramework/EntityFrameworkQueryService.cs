using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.KeyMappers;
using Microsoft.EntityFrameworkCore;

namespace EntityAxis.EntityFramework;

/// <summary>
/// Generic Entity Framework implementation of <see cref="IQueryService{TEntity, TKey}"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the application entity.</typeparam>
/// <typeparam name="TDbEntity">The type of the database entity.</typeparam>
/// <typeparam name="TDbContext">The type of the Entity Framework DbContext.</typeparam>
/// <typeparam name="TKey">The type of the application entity's identifier.</typeparam>
/// <typeparam name="TDbKey">The type of the database entity's identifier.</typeparam>
public class EntityFrameworkQueryService<TEntity, TDbEntity, TDbContext, TKey, TDbKey> : EntityFrameworkServiceBase<TDbContext>, IQueryService<TEntity, TKey>
    where TEntity : class, IEntityId<TKey>
    where TDbEntity : class, IEntityId<TDbKey>
    where TDbContext : DbContext
{
    private readonly IKeyMapper<TKey, TDbKey> _keyMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkQueryService{TEntity, TDbEntity, TDbContext, TKey, TDbKey}"/> class.
    /// </summary>
    /// <param name="contextFactory">The DbContext factory.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="keyMapper">Mapper for converting between application and database key types.</param>
    public EntityFrameworkQueryService(IDbContextFactory<TDbContext> contextFactory, IMapper mapper, IKeyMapper<TKey, TDbKey> keyMapper)
        : base(contextFactory, mapper)
    {
        _keyMapper = keyMapper;
    }

    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbSet = context.Set<TDbEntity>();
        TDbKey dbKey = _keyMapper.ToDbKey(id);
        if (dbKey is null)
        {
            throw new InvalidOperationException("The entity's key cannot be null.");
        }
        var entity = await dbSet.FindAsync(keyValues: new object[] { dbKey }, cancellationToken);
        return Mapper.Map<TEntity>(entity);
    }

    /// <inheritdoc/>
    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbSet = context.Set<TDbEntity>();
        var entities = await dbSet.ToListAsync(cancellationToken);
        return Mapper.Map<List<TEntity>>(entities);
    }

    /// <inheritdoc/>
    public async Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be at least 1.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be at least 1.");

        cancellationToken.ThrowIfCancellationRequested();

        var context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        var dbSet = context.Set<TDbEntity>();

        var totalItemCount = await dbSet.CountAsync(cancellationToken);
        var skip = (page - 1) * pageSize;

        var entities = await dbSet
            .AsNoTracking()
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var mapped = Mapper.Map<List<TEntity>>(entities);

        return new PagedResult<TEntity>(mapped, totalItemCount, page, pageSize);
    }
}
