using EntityAxis.Abstractions;
using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Registration.Tests.TestClasses;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EntityAxis.MediatR.Registration.Tests;

public class EntityQueryHandlerBuilderTests
{
    private static IServiceProvider BuildServiceProvider(Action<EntityQueryHandlerBuilder<TestEntity, int>> configure)
    {
        var services = new ServiceCollection();

        // Required mocks
        services.AddTransient<IGetById<TestEntity, int>>(_ => Mock.Of<IGetById<TestEntity, int>>());
        services.AddTransient<IGetAll<TestEntity, int>>(_ => Mock.Of<IGetAll<TestEntity, int>>());
        services.AddTransient<IGetPaged<TestEntity, int>>(_ => Mock.Of<IGetPaged<TestEntity, int>>());

        var builder = new EntityQueryHandlerBuilder<TestEntity, int>(services);
        configure(builder);

        return services.BuildServiceProvider();
    }

    [Fact]
    public void AddGetById_Should_Register_Handler_And_Validator()
    {
        var provider = BuildServiceProvider(builder => builder.AddGetById());

        provider.GetService<IRequestHandler<GetEntityByIdQuery<TestEntity, int>, TestEntity?>>()
            .Should().NotBeNull();

        provider.GetService<IValidator<GetEntityByIdQuery<TestEntity, int>>>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddGetAll_Should_Register_Handler()
    {
        var provider = BuildServiceProvider(builder => builder.AddGetAll());

        provider.GetService<IRequestHandler<GetAllEntitiesQuery<TestEntity, int>, List<TestEntity>>>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddGetPaged_Should_Register_Handler_And_Validator()
    {
        var provider = BuildServiceProvider(builder => builder.AddGetPaged());

        provider.GetService<IRequestHandler<GetPagedEntitiesQuery<TestEntity, int>, PagedResult<TestEntity>>>()
            .Should().NotBeNull();

        provider.GetService<IValidator<GetPagedEntitiesQuery<TestEntity, int>>>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddAllQueries_Should_Register_All_Handlers_And_Validators()
    {
        var provider = BuildServiceProvider(builder => builder.AddAllQueries());

        provider.GetService<IRequestHandler<GetEntityByIdQuery<TestEntity, int>, TestEntity?>>()
            .Should().NotBeNull();

        provider.GetService<IValidator<GetEntityByIdQuery<TestEntity, int>>>()
            .Should().NotBeNull();

        provider.GetService<IRequestHandler<GetAllEntitiesQuery<TestEntity, int>, List<TestEntity>>>()
            .Should().NotBeNull();

        provider.GetService<IRequestHandler<GetPagedEntitiesQuery<TestEntity, int>, PagedResult<TestEntity>>>()
            .Should().NotBeNull();

        provider.GetService<IValidator<GetPagedEntitiesQuery<TestEntity, int>>>()
            .Should().NotBeNull();
    }
}
