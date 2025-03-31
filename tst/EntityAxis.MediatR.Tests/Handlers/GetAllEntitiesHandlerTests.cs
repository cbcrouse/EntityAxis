using EntityAxis.Abstractions;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Tests.TestClasses;
using FluentAssertions;
using Moq;

namespace EntityAxis.MediatR.Tests.Handlers;

public class GetAllEntitiesHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Entities()
    {
        var data = new List<TestEntity>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        var service = new Mock<IGetAll<TestEntity, int>>();
        service.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        var handler = new GetAllEntitiesHandler<TestEntity, int>(service.Object);
        var result = await handler.Handle(new GetAllEntitiesQuery<TestEntity, int>(), CancellationToken.None);

        result.Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task Handle_Should_Respect_CancellationToken()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var service = new Mock<IGetAll<TestEntity, int>>();
        var handler = new GetAllEntitiesHandler<TestEntity, int>(service.Object);
        var query = new GetAllEntitiesQuery<TestEntity, int>();

        var act = async () => await handler.Handle(query, cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}