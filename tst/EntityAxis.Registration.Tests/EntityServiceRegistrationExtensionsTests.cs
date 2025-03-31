using EntityAxis.Abstractions;
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
            TestEntityService,
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
            TestEntityService,
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
        services.AddEntityAxisCommandAndQueryServicesFromAssembly<TestEntityService>();

        var provider = services.BuildServiceProvider();

        // Assert: only generic types are registered via scanning
        provider.GetService<ICommandService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IQueryService<TestEntity, int>>().Should().NotBeNull();
    }

    [Fact]
    public void AddEntityAxisCommandAndQueryServicesFromAssemblies_Should_Scan_And_Register_Generic_Types()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntityAxisCommandAndQueryServicesFromAssemblies([typeof(TestEntityService).Assembly]);

        var provider = services.BuildServiceProvider();

        // Assert: only generic types are registered via scanning
        provider.GetService<ICommandService<TestEntity, int>>().Should().NotBeNull();
        provider.GetService<IQueryService<TestEntity, int>>().Should().NotBeNull();
    }
}
