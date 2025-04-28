using EntityAxis.Registration.Tests.MockAssembly;

namespace EntityAxis.Registration.Tests.TestClasses;

public class TestEntityCommandService : ITestCommandService
{
    public Task<int> CreateAsync(TestEntity entity, CancellationToken cancellationToken = default)
        => Task.FromResult(entity.Id);

    public Task<int> UpdateAsync(TestEntity entity, CancellationToken cancellationToken = default)
        => Task.FromResult(entity.Id);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
