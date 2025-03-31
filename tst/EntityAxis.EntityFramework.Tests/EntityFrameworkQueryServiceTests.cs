using AutoMapper;
using EntityAxis.EntityFramework.Tests.TestClasses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EntityAxis.EntityFramework.Tests;

public class EntityFrameworkQueryServiceTests
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<TestDbContext> _contextFactory;

    public EntityFrameworkQueryServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TestProductEntity, TestProduct>().ReverseMap();
        });

        _mapper = config.CreateMapper();

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase($"TestQueryDb_{Guid.NewGuid()}")
            .Options;

        var context = new TestDbContext(options);

        _contextFactory = Mock.Of<IDbContextFactory<TestDbContext>>(f =>
            f.CreateDbContextAsync(It.IsAny<CancellationToken>()) == Task.FromResult(context));
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnEntity_WhenExists()
    {
        var service = new EntityFrameworkQueryService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);
        var context = await _contextFactory.CreateDbContextAsync();

        context.Products.Add(new TestProductEntity
        {
            Id = 1,
            Name = "Query Me",
            Description = "From DB",
            Price = 99
        });
        await context.SaveChangesAsync();

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_WhenNotFound()
    {
        var service = new EntityFrameworkQueryService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnAllEntities()
    {
        var service = new EntityFrameworkQueryService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);
        var context = await _contextFactory.CreateDbContextAsync();

        context.Products.AddRange(
            new TestProductEntity { Id = 1, Name = "One" },
            new TestProductEntity { Id = 2, Name = "Two" });

        await context.SaveChangesAsync();

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Select(x => x.Name).Should().Contain(expected:["One", "Two"]);
    }

    [Fact]
    public async Task GetPagedAsync_Should_ReturnCorrectSubset()
    {
        var service = new EntityFrameworkQueryService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);
        var context = await _contextFactory.CreateDbContextAsync();

        for (int i = 1; i <= 10; i++)
        {
            context.Products.Add(new TestProductEntity { Id = i, Name = $"Product {i}" });
        }

        await context.SaveChangesAsync();

        var result = await service.GetPagedAsync(page: 2, pageSize: 3);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalItemCount.Should().Be(10);
        result.PageNumber.Should().Be(2);
        result.TotalPages.Should().Be(4); // 10 / 3 = 3.33 → 4 pages
    }

    [Fact]
    public async Task GetPagedAsync_Should_Throw_WhenPageOrPageSizeIsInvalid()
    {
        var service = new EntityFrameworkQueryService<TestProduct, TestProductEntity, TestDbContext, int>(_contextFactory, _mapper);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetPagedAsync(0, 5));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetPagedAsync(1, 0));
    }
}
