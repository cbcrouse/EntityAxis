using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Exceptions;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Tests.TestClasses;
using FluentAssertions;
using Moq;

namespace EntityAxis.MediatR.Tests.Handlers;

public class DeleteEntityHandlerTests
{
    [Fact]
    public async Task Handle_Should_Throw_When_Cancelled()
    {
        // Arrange
        var entityId = 5;
        var entity = new TestEntity { Id = entityId };

        var getByIdService = new Mock<IGetById<TestEntity, int>>();
        var deleteService = new Mock<IDelete<TestEntity, int>>();
        getByIdService.Setup(s => s.GetByIdAsync(entityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var handler = new DeleteEntityHandler<TestEntity, int>(
            getByIdService.Object,
            deleteService.Object
        );

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var command = new DeleteEntityCommand<TestEntity, int>(entityId);

        // Act
        var act = async () => await handler.Handle(command, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Handle_Should_Call_GetById_And_Delete()
    {
        // Arrange
        int entityId = 99;
        var entity = new TestEntity { Id = entityId };

        var getById = new Mock<IGetById<TestEntity, int>>();
        getById.Setup(s => s.GetByIdAsync(entityId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(entity);

        var delete = new Mock<IDelete<TestEntity, int>>();
        delete.Setup(s => s.DeleteAsync(entityId, It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);

        var handler = new DeleteEntityHandler<TestEntity, int>(getById.Object, delete.Object);
        var command = new DeleteEntityCommand<TestEntity, int>(entityId);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        getById.Verify(s => s.GetByIdAsync(entityId, It.IsAny<CancellationToken>()), Times.Once);
        delete.Verify(s => s.DeleteAsync(entityId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_EntityNotFound_When_Entity_DoesNotExist()
    {
        // Arrange
        int missingId = 123;

        var getById = new Mock<IGetById<TestEntity, int>>();
        getById.Setup(s => s.GetByIdAsync(missingId, It.IsAny<CancellationToken>()))
               .ReturnsAsync((TestEntity?)null);

        var delete = new Mock<IDelete<TestEntity, int>>();

        var handler = new DeleteEntityHandler<TestEntity, int>(getById.Object, delete.Object);
        var command = new DeleteEntityCommand<TestEntity, int>(missingId);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage($"Unable to find {nameof(TestEntity)} with ID \"{missingId}\".");

        delete.Verify(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
