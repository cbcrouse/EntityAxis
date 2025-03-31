using EntityAxis.Abstractions;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Tests.TestClasses;
using FluentAssertions;
using Moq;

namespace EntityAxis.MediatR.Tests.Handlers;

public class GetEntityByIdHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Entity_When_Found()
    {
        var entity = new TestEntity { Id = 1 };
        var service = new Mock<IGetById<TestEntity, int>>();
        service.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var handler = new GetEntityByIdHandler<TestEntity, int>(service.Object);
        var result = await handler.Handle(new GetEntityByIdQuery<TestEntity, int>(1), CancellationToken.None);

        result.Should().BeSameAs(entity);
    }

    [Fact]
    public async Task Handle_Should_Respect_CancellationToken()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var service = new Mock<IGetById<TestEntity, int>>();
        var handler = new GetEntityByIdHandler<TestEntity, int>(service.Object);
        var query = new GetEntityByIdQuery<TestEntity, int>(1);

        var act = async () => await handler.Handle(query, cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}