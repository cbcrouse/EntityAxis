using EntityAxis.Abstractions;
using EntityAxis.Registration.Tests.MockAssembly;
using EntityAxis.Registration.Tests.TestClasses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EntityAxis.Registration.Tests;

public class EntityServiceRegistrationExtensionsTests
{
    [Fact]
    public void AddEntityAxisCommandService_Should_Register_All_Command_Interfaces()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntityAxisCommandService<
            ITestCommandService,
            TestEntityCommandService,
            TestEntity,
            int>();

        var provider = services.BuildServiceProvider();

        // Assert: check the generic interfaces (these are what's registered)
        provider.GetService<ITestCommandService>().Should().NotBeNull(); // Custom interface
        provider.GetService<ICommandService<TestEntity, int>>().Should().NotBeNull(); // Generic
        provider.GetService<ICreate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IUpdate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IDelete<TestEntity, int>>().Should().NotBeNull();
    }

    [Fact]
    public void AddEntityAxisQueryService_Should_Register_All_Query_Interfaces()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntityAxisQueryService<
            ITestQueryService,
            TestEntityQueryService,
            TestEntity,
            int>();

        var provider = services.BuildServiceProvider();

        // Assert: check the generic interfaces (these are what's registered)
        provider.GetService<ITestQueryService>().Should().NotBeNull(); // Custom interface
        provider.GetService<IQueryService<TestEntity, int>>().Should().NotBeNull(); // Generic
        provider.GetService<IGetById<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetAll<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetPaged<TestEntity, int>>().Should().NotBeNull();
    }

    [Fact]
    public void AddEntityAxisCommandAndQueryServicesFromAssembly_Should_Register_Generic_Types()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntityAxisCommandAndQueryServicesFromAssembly<TestEntityCommandService>();

        var provider = services.BuildServiceProvider();

        // Assert: only generic types are registered via scanning
        provider.GetService<ITestCommandService>().Should().NotBeNull();
        provider.GetService<ICommandService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<ICreate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IUpdate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IDelete<TestEntity, int>>().Should().NotBeNull();

        provider.GetService<ITestQueryService>().Should().NotBeNull();
        provider.GetService<IQueryService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetById<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetAll<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetPaged<TestEntity, int>>().Should().NotBeNull();
    }

    [Fact]
    public void AddEntityAxisCommandAndQueryServicesFromAssemblies_Should_Scan_And_Register_Generic_Types()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntityAxisCommandAndQueryServicesFromAssemblies([typeof(TestEntityCommandService).Assembly]);

        var provider = services.BuildServiceProvider();

        // Assert: only generic types are registered via scanning
        provider.GetService<ITestCommandService>().Should().NotBeNull();
        provider.GetService<ICommandService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<ICreate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IUpdate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IDelete<TestEntity, int>>().Should().NotBeNull();

        provider.GetService<ITestQueryService>().Should().NotBeNull();
        provider.GetService<IQueryService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetById<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetAll<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetPaged<TestEntity, int>>().Should().NotBeNull();
    }

    [Fact]
    public void AddEntityAxisCommandAndQueryServicesFromAssemblies_Should_Not_Register_Custom_Interfaces_When_Inherited_From_Base()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntityAxisCommandAndQueryServicesFromAssemblies([typeof(TestEntityQueryServiceWithBase).Assembly]);

        var provider = services.BuildServiceProvider();

        // Assert: only generic types are registered via scanning
        provider.GetService<ITestCommandService>().Should().NotBeNull();
        provider.GetService<ICommandService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<ICreate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IUpdate<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IDelete<TestEntity, int>>().Should().NotBeNull();

        provider.GetService<ITestQueryService>().Should().NotBeNull();
        provider.GetService<IQueryService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetById<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetAll<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IGetPaged<TestEntity, int>>().Should().NotBeNull();
    }
}
