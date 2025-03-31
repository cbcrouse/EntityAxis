using EntityAxis.Abstractions;

namespace EntityAxis.Registration.Tests.TestClasses;

public interface ITestCommandService : ICommandService<TestEntity, int>;
public interface ITestQueryService : IQueryService<TestEntity, int>;

public class TestEntityService : ITestCommandService, ITestQueryService
{
    public Task<int> CreateAsync(TestEntity entity, CancellationToken cancellationToken = default)
        => Task.FromResult(entity.Id);

    public Task<int> UpdateAsync(TestEntity entity, CancellationToken cancellationToken = default)
        => Task.FromResult(entity.Id);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

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
