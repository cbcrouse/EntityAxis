using Microsoft.EntityFrameworkCore;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<ProductDbEntity> Products => Set<ProductDbEntity>();
}