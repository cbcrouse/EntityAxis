using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Exceptions;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Tests.TestClasses;
using FluentAssertions;
using Moq;

namespace EntityAxis.MediatR.Tests.Handlers;

public class UpdateEntityHandlerTests
{
    [Fact]
    public async Task Handle_Should_Throw_When_Cancelled()
    {
        // Arrange
        var model = new TestUpdateModel { Id = 1 };
        var entity = new TestEntity { Id = 1 };

        var updateService = new Mock<IUpdate<TestEntity, int>>();
        var getByIdService = new Mock<IGetById<TestEntity, int>>();
        var mapper = new Mock<IMapper>();
        mapper.Setup(m => m.Map<TestEntity>(model)).Returns(entity);

        var handler = new UpdateEntityHandler<TestUpdateModel, TestEntity, int>(
            getByIdService.Object,
            updateService.Object,
            mapper.Object
        );

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var command = new UpdateEntityCommand<TestUpdateModel, TestEntity, int>(model);

        // Act
        var act = async () => await handler.Handle(command, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Handle_Should_Call_UpdateService_And_Return_Id()
    {
        // Arrange
        var updateModel = new TestUpdateModel { Id = 99 };
        var mappedEntity = new TestEntity { Id = 99 };

        var mapper = new Mock<IMapper>();
        mapper.Setup(m => m.Map<TestEntity>(updateModel)).Returns(mappedEntity);

        var updateService = new Mock<IUpdate<TestEntity, int>>();
        updateService.Setup(s => s.UpdateAsync(mappedEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(99);

        var getByIdService = new Mock<IGetById<TestEntity, int>>();
        getByIdService.Setup(s => s.GetByIdAsync(updateModel.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mappedEntity); // or new TestEntity { Id = 99 }

        var handler = new UpdateEntityHandler<TestUpdateModel, TestEntity, int>(
            getByIdService.Object,
            updateService.Object,
            mapper.Object
        );

        var command = new UpdateEntityCommand<TestUpdateModel, TestEntity, int>(updateModel);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().Be(99);
        updateService.Verify(s => s.UpdateAsync(mappedEntity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_UpdateService_Throws()
    {
        // Arrange
        var updateModel = new TestUpdateModel { Id = 123 };
        var mappedEntity = new TestEntity { Id = 123 };

        var mapper = new Mock<IMapper>();
        mapper.Setup(m => m.Map<TestEntity>(updateModel)).Returns(mappedEntity);

        var updateService = new Mock<IUpdate<TestEntity, int>>();
        updateService.Setup(s => s.UpdateAsync(mappedEntity, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException(nameof(TestEntity), updateModel.Id));

        var getByIdService = new Mock<IGetById<TestEntity, int>>();

        var handler = new UpdateEntityHandler<TestUpdateModel, TestEntity, int>(
            getByIdService.Object,
            updateService.Object,
            mapper.Object
        );

        var command = new UpdateEntityCommand<TestUpdateModel, TestEntity, int>(updateModel);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage($"Unable to find {nameof(TestEntity)} with ID \"{updateModel.Id}\".");
    }
}