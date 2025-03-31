using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Registration.Tests.TestClasses;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EntityAxis.MediatR.Registration.Tests;

public class HandlerRegistrationExtensionsTests
{
    private static IServiceCollection CreateServiceCollectionWithMocks()
    {
        var services = new ServiceCollection();

        services.AddTransient<ICreate<TestEntity, int>>(_ => Mock.Of<ICreate<TestEntity, int>>());
        services.AddTransient<IUpdate<TestEntity, int>>(_ => Mock.Of<IUpdate<TestEntity, int>>());
        services.AddTransient<IDelete<TestEntity, int>>(_ => Mock.Of<IDelete<TestEntity, int>>());
        services.AddTransient<IGetById<TestEntity, int>>(_ => Mock.Of<IGetById<TestEntity, int>>());
        services.AddTransient<IGetAll<TestEntity, int>>(_ => Mock.Of<IGetAll<TestEntity, int>>());
        services.AddTransient<IGetPaged<TestEntity, int>>(_ => Mock.Of<IGetPaged<TestEntity, int>>());
        services.AddTransient<IMapper>(_ => Mock.Of<IMapper>());
        services.AddTransient<IValidator<TestCreateModel>>(_ => Mock.Of<IValidator<TestCreateModel>>());
        services.AddTransient<IValidator<TestUpdateModel>>(_ => Mock.Of<IValidator<TestUpdateModel>>());

        return services;
    }

    [Fact]
    public void AddEntityAxisCommandHandlers_Should_Register_All_Command_Handlers_And_Validators()
    {
        // Arrange
        var services = CreateServiceCollectionWithMocks();

        // Act
        services.AddEntityAxisCommandHandlers<TestCreateModel, TestUpdateModel, TestEntity, int>();
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IRequestHandler<CreateEntityCommand<TestCreateModel, TestEntity, int>, int>>().Should().NotBeNull();
        provider.GetService<IValidator<CreateEntityCommand<TestCreateModel, TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<UpdateEntityCommand<TestUpdateModel, TestEntity, int>, int>>().Should().NotBeNull();
        provider.GetService<IValidator<UpdateEntityCommand<TestUpdateModel, TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<DeleteEntityCommand<TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IValidator<DeleteEntityCommand<TestEntity, int>>>().Should().NotBeNull();
    }

    [Fact]
    public void AddEntityAxisQueryHandlers_Should_Register_All_Query_Handlers_And_Validators()
    {
        // Arrange
        var services = CreateServiceCollectionWithMocks();

        // Act
        services.AddEntityAxisQueryHandlers<TestEntity, int>();
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IRequestHandler<GetEntityByIdQuery<TestEntity, int>, TestEntity?>>().Should().NotBeNull();
        provider.GetService<IValidator<GetEntityByIdQuery<TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<GetAllEntitiesQuery<TestEntity, int>, List<TestEntity>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<GetPagedEntitiesQuery<TestEntity, int>, PagedResult<TestEntity>>>().Should().NotBeNull();
        provider.GetService<IValidator<GetPagedEntitiesQuery<TestEntity, int>>>().Should().NotBeNull();
    }

    [Fact]
    public void AddEntityAxisHandlers_Should_Register_All_Command_And_Query_Handlers()
    {
        // Arrange
        var services = CreateServiceCollectionWithMocks();

        // Act
        services.AddEntityAxisHandlers<TestCreateModel, TestUpdateModel, TestEntity, int>();
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IRequestHandler<CreateEntityCommand<TestCreateModel, TestEntity, int>, int>>().Should().NotBeNull();
        provider.GetService<IValidator<CreateEntityCommand<TestCreateModel, TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<UpdateEntityCommand<TestUpdateModel, TestEntity, int>, int>>().Should().NotBeNull();
        provider.GetService<IValidator<UpdateEntityCommand<TestUpdateModel, TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<DeleteEntityCommand<TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IValidator<DeleteEntityCommand<TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<GetEntityByIdQuery<TestEntity, int>, TestEntity?>>().Should().NotBeNull();
        provider.GetService<IValidator<GetEntityByIdQuery<TestEntity, int>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<GetAllEntitiesQuery<TestEntity, int>, List<TestEntity>>>().Should().NotBeNull();
        provider.GetService<IRequestHandler<GetPagedEntitiesQuery<TestEntity, int>, PagedResult<TestEntity>>>().Should().NotBeNull();
        provider.GetService<IValidator<GetPagedEntitiesQuery<TestEntity, int>>>().Should().NotBeNull();
    }
}