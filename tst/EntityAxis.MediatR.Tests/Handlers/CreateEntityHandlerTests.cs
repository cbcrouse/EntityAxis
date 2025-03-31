using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Tests.TestClasses;
using FluentAssertions;
using Moq;

namespace EntityAxis.MediatR.Tests.Handlers;

public class CreateEntityHandlerTests
{
    [Fact]
    public async Task Handle_Should_Throw_When_Canceled()
    {
        // Arrange
        var model = new TestCreateModel();
        var mapper = new Mock<IMapper>();
        var createService = new Mock<ICreate<TestEntity, int>>();

        var handler = new CreateEntityHandler<TestCreateModel, TestEntity, int>(
            createService.Object,
            mapper.Object
        );

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var command = new CreateEntityCommand<TestCreateModel, TestEntity, int>(model);

        // Act
        var act = async () => await handler.Handle(command, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Handle_Should_Call_CreateService_And_Return_Id()
    {
        // Arrange
        var createModel = new TestCreateModel { Id = 42 };
        var mappedEntity = new TestEntity { Id = 42 };

        var mapper = new Mock<IMapper>();
        var createService = new Mock<ICreate<TestEntity, int>>();

        mapper.Setup(m => m.Map<TestEntity>(createModel)).Returns(mappedEntity);
        createService.Setup(s => s.CreateAsync(mappedEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(42);

        var handler = new CreateEntityHandler<TestCreateModel, TestEntity, int>(
            createService.Object, mapper.Object
        );

        var command = new CreateEntityCommand<TestCreateModel, TestEntity, int>(createModel);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(42);
        createService.Verify(s => s.CreateAsync(mappedEntity, It.IsAny<CancellationToken>()), Times.Once);
    }
}
