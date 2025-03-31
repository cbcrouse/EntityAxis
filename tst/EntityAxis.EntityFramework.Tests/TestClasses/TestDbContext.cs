using Microsoft.EntityFrameworkCore;

namespace EntityAxis.EntityFramework.Tests.TestClasses;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<TestProductEntity> Products => Set<TestProductEntity>();

    public override void Dispose()
    {
        // Do not dispose so we can reuse the context during tests
    }

    public override ValueTask DisposeAsync()
    {
        // Do not dispose so we can reuse the context during tests
        return ValueTask.CompletedTask;
    }
}