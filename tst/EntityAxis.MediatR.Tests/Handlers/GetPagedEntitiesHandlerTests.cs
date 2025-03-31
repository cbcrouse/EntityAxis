using EntityAxis.Abstractions;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Tests.TestClasses;
using FluentAssertions;
using Moq;

namespace EntityAxis.MediatR.Tests.Handlers;

public class GetPagedEntitiesHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_PagedResult()
    {
        var expected = new PagedResult<TestEntity>(
            [
                new TestEntity {Id = 1},
                new TestEntity {Id = 2}
            ],
            totalItemCount: 10,
            pageNumber: 1,
            pageSize: 2
        );

        var service = new Mock<IGetPaged<TestEntity, int>>();
        service.Setup(s => s.GetPagedAsync(1, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetPagedEntitiesHandler<TestEntity, int>(service.Object);
        var query = new GetPagedEntitiesQuery<TestEntity, int>(1, 2);

        var result = await handler.Handle(query, CancellationToken.None);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Handle_Should_Respect_CancellationToken()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var service = new Mock<IGetPaged<TestEntity, int>>();
        var handler = new GetPagedEntitiesHandler<TestEntity, int>(service.Object);
        var query = new GetPagedEntitiesQuery<TestEntity, int>(1, 2);

        var act = async () => await handler.Handle(query, cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}