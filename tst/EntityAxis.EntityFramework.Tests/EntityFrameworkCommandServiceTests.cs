using AutoMapper;
using EntityAxis.EntityFramework.Tests.TestClasses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EntityAxis.EntityFramework.Tests;

public class EntityFrameworkCommandServiceTests
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<TestDbContext> _contextFactory;

    public EntityFrameworkCommandServiceTests()
    {
        // AutoMapper config for test types
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TestProduct, TestProductEntity>().ReverseMap();
        });

        _mapper = config.CreateMapper();

        // In-memory EF context
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        var context = new TestDbContext(options);

        // Always return the same instance (mocked factory)
        _contextFactory = Mock.Of<IDbContextFactory<TestDbContext>>(f =>
            f.CreateDbContextAsync(It.IsAny<CancellationToken>()) == Task.FromResult(context));
    }

    [Fact]
    public async Task CreateAsync_Should_SaveEntity_AndReturnId()
    {
        // Arrange
        var service = new EntityFrameworkCommandService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);

        var product = new TestProduct
        {
            Id = 1,
            Name = "Test Product",
            Description = "Desc",
            Price = 19.99m
        };

        // Act
        var id = await service.CreateAsync(product);

        // Assert
        id.Should().Be(1);

        var context = await _contextFactory.CreateDbContextAsync();
        var saved = await context.Products.FindAsync(1);
        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task UpdateAsync_Should_ModifyExistingEntity_AndReturnId()
    {
        // Arrange
        var service = new EntityFrameworkCommandService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);

        var context = await _contextFactory.CreateDbContextAsync();
        context.Products.Add(new TestProductEntity
        {
            Id = 2,
            Name = "Original Name",
            Description = "Original Desc",
            Price = 5.00m
        });
        await context.SaveChangesAsync();

        var updated = new TestProduct
        {
            Id = 2,
            Name = "Updated Name",
            Description = "Updated Desc",
            Price = 9.99m
        };

        // Act
        var id = await service.UpdateAsync(updated);

        // Assert
        id.Should().Be(2);

        var saved = await context.Products.FindAsync(2);
        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Updated Name");
        saved.Description.Should().Be("Updated Desc");
        saved.Price.Should().Be(9.99m);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_WhenEntityNotFound()
    {
        // Arrange
        var service = new EntityFrameworkCommandService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);

        var nonexistent = new TestProduct
        {
            Id = 999,
            Name = "Does Not Exist",
            Description = "Missing",
            Price = 1.23m
        };

        // Act
        var act = async () => await service.UpdateAsync(nonexistent);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*999*");
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveEntity_WhenExists()
    {
        // Arrange
        var service = new EntityFrameworkCommandService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);

        var context = await _contextFactory.CreateDbContextAsync();
        context.Products.Add(new TestProductEntity
        {
            Id = 3,
            Name = "To Delete",
            Description = "Soon gone",
            Price = 0.99m
        });
        await context.SaveChangesAsync();

        // Act
        await service.DeleteAsync(3);

        // Assert
        var deleted = await context.Products.FindAsync(3);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_DoNothing_WhenEntityNotFound()
    {
        // Arrange
        var service = new EntityFrameworkCommandService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);

        // Act (should not throw)
        var act = async () => await service.DeleteAsync(404);

        // Assert
        await act.Should().NotThrowAsync();
    }
}
