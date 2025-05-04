using EntityAxis.Abstractions;
using EntityAxis.Registration.Tests.MockAssembly;

namespace EntityAxis.Registration.Tests.TestClasses;

internal class TestEntityQueryService : ITestQueryService
{
    public Task<TestEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => Task.FromResult<TestEntity?>(new() { Id = id });

    public Task<List<TestEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new List<TestEntity>());

    public Task<PagedResult<TestEntity>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = new List<TestEntity>(); // Empty page for testing
        return Task.FromResult(new PagedResult<TestEntity>(
            items: items,
            totalItemCount: 0,
            pageNumber: page,
            pageSize: pageSize));
    }
}

internal class TestEntityQueryServiceWithBase : TestEntityFrameworkQueryService<TestEntity, int, int>, ITestQueryService
{
    // No need to reimplement methods — inherited methods throw NotImplementedException
}

internal abstract class TestEntityFrameworkQueryService<TEntity, TKey, TDbKey> : IQueryService<TEntity, TKey>
    where TEntity : class, IEntityId<TKey>
{
    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
