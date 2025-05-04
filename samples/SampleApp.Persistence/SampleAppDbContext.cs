using Microsoft.EntityFrameworkCore;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

public class SampleAppDbContext(DbContextOptions<SampleAppDbContext> options) : DbContext(options)
{
    public DbSet<ProductDbEntity> Products => Set<ProductDbEntity>();
    public DbSet<OrderDbEntity> Orders => Set<OrderDbEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial data
        modelBuilder.Entity<ProductDbEntity>().HasData(
            new ProductDbEntity { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.00m },
            new ProductDbEntity { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.00m }
        );

        modelBuilder.Entity<OrderDbEntity>().HasData(
            new OrderDbEntity { Id = Guid.NewGuid(), CustomerName = "John Doe", TotalAmount = 30.00m }
        );
    }
}