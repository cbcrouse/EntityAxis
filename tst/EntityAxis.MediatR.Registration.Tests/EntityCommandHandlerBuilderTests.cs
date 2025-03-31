using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Registration.Tests.TestClasses;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EntityAxis.MediatR.Registration.Tests;

public class EntityCommandHandlerBuilderTests
{
    private static ServiceCollection CreateServiceCollectionWithCommonDependencies()
    {
        var services = new ServiceCollection();

        services.AddTransient<ICreate<TestEntity, int>>(_ => Mock.Of<ICreate<TestEntity, int>>());
        services.AddTransient<IUpdate<TestEntity, int>>(_ => Mock.Of<IUpdate<TestEntity, int>>());
        services.AddTransient<IDelete<TestEntity, int>>(_ => Mock.Of<IDelete<TestEntity, int>>());
        services.AddTransient<IGetById<TestEntity, int>>(_ => Mock.Of<IGetById<TestEntity, int>>());
        services.AddTransient<IMapper>(_ => Mock.Of<IMapper>());
        services.AddTransient<IValidator<TestCreateModel>>(_ => Mock.Of<IValidator<TestCreateModel>>());
        services.AddTransient<IValidator<TestUpdateModel>>(_ => Mock.Of<IValidator<TestUpdateModel>>());

        return services;
    }

    [Fact]
    public void AddCreate_Should_Register_Create_Handler_And_Validator()
    {
        // Arrange
        var services = CreateServiceCollectionWithCommonDependencies();

        // Act
        new EntityCommandHandlerBuilder<TestEntity, int>(services)
            .AddCreate<TestCreateModel>();

        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IRequestHandler<CreateEntityCommand<TestCreateModel, TestEntity, int>, int>>()
            .Should().NotBeNull();

        provider.GetService<IValidator<CreateEntityCommand<TestCreateModel, TestEntity, int>>>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddUpdate_Should_Register_Update_Handler_And_Validator()
    {
        // Arrange
        var services = CreateServiceCollectionWithCommonDependencies();

        // Act
        new EntityCommandHandlerBuilder<TestEntity, int>(services)
            .AddUpdate<TestUpdateModel>();

        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IRequestHandler<UpdateEntityCommand<TestUpdateModel, TestEntity, int>, int>>()
            .Should().NotBeNull();

        provider.GetService<IValidator<UpdateEntityCommand<TestUpdateModel, TestEntity, int>>>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddDelete_Should_Register_Delete_Handler_And_Validator()
    {
        // Arrange
        var services = CreateServiceCollectionWithCommonDependencies();

        // Act
        new EntityCommandHandlerBuilder<TestEntity, int>(services)
            .AddDelete();

        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IRequestHandler<DeleteEntityCommand<TestEntity, int>>>()
            .Should().NotBeNull();

        provider.GetService<IValidator<DeleteEntityCommand<TestEntity, int>>>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddAllCommands_Should_Register_All_Handlers_And_Validators()
    {
        // Arrange
        var services = CreateServiceCollectionWithCommonDependencies();

        // Act
        new EntityCommandHandlerBuilder<TestEntity, int>(services)
            .AddAllCommands<TestCreateModel, TestUpdateModel>();

        var provider = services.BuildServiceProvider();

        // Assert Create
        provider.GetService<IRequestHandler<CreateEntityCommand<TestCreateModel, TestEntity, int>, int>>()
            .Should().NotBeNull();
        provider.GetService<IValidator<CreateEntityCommand<TestCreateModel, TestEntity, int>>>()
            .Should().NotBeNull();

        // Assert Update
        provider.GetService<IRequestHandler<UpdateEntityCommand<TestUpdateModel, TestEntity, int>, int>>()
            .Should().NotBeNull();
        provider.GetService<IValidator<UpdateEntityCommand<TestUpdateModel, TestEntity, int>>>()
            .Should().NotBeNull();

        // Assert Delete
        provider.GetService<IRequestHandler<DeleteEntityCommand<TestEntity, int>>>()
            .Should().NotBeNull();
        provider.GetService<IValidator<DeleteEntityCommand<TestEntity, int>>>()
            .Should().NotBeNull();
    }
}
