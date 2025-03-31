using FluentAssertions;

namespace EntityAxis.Abstractions.Tests;

public class InterfaceContractTests
{
    [Fact]
    public void ICommandService_ShouldImplement_AllCommandInterfaces()
    {
        var type = typeof(ICommandService<,>);
        var interfaces = type.GetInterfaces()
            .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i)
            .ToList();

        interfaces.Should().Contain(typeof(ICreate<,>));
        interfaces.Should().Contain(typeof(IUpdate<,>));
        interfaces.Should().Contain(typeof(IDelete<,>));
    }

    [Fact]
    public void IQueryService_ShouldImplement_AllQueryInterfaces()
    {
        var type = typeof(IQueryService<,>);
        var interfaces = type.GetInterfaces()
            .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i)
            .ToList();

        interfaces.Should().Contain(typeof(IGetById<,>));
        interfaces.Should().Contain(typeof(IGetAll<,>));
        interfaces.Should().Contain(typeof(IGetPaged<,>));
    }
}
