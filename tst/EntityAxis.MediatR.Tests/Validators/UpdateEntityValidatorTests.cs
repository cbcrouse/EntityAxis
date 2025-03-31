using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Tests.TestClasses;
using EntityAxis.MediatR.Validators;
using FluentAssertions;
using FluentValidation;

namespace EntityAxis.MediatR.Tests.Validators;

public class UpdateEntityValidatorTests
{
    [Fact]
    public void Validate_Should_Return_NoErrors_When_Model_Is_Valid()
    {
        // Arrange
        var modelValidator = new InlineValidator<TestUpdateModel>();
        modelValidator.RuleFor(x => x.Id).GreaterThan(0);

        var validator = new UpdateEntityValidator<TestUpdateModel, TestEntity, int>(modelValidator);
        var command = new UpdateEntityCommand<TestUpdateModel, TestEntity, int>(new TestUpdateModel { Id = 1 });

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Should_Return_Error_When_Model_Is_Invalid()
    {
        // Arrange
        var modelValidator = new InlineValidator<TestUpdateModel>();
        modelValidator.RuleFor(x => x.Id).GreaterThan(0);

        var validator = new UpdateEntityValidator<TestUpdateModel, TestEntity, int>(modelValidator);
        var command = new UpdateEntityCommand<TestUpdateModel, TestEntity, int>(new TestUpdateModel { Id = 0 });

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "UpdateModel.Id");
    }

    [Fact]
    public void Validate_Should_Return_Error_When_No_ModelValidator_Provided()
    {
        // Arrange
        var validator = new UpdateEntityValidator<TestUpdateModel, TestEntity, int>(null);
        var command = new UpdateEntityCommand<TestUpdateModel, TestEntity, int>(new TestUpdateModel { Id = 123 });

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.ErrorMessage.Contains($"No validator registered for {nameof(TestUpdateModel)}"));
    }
}
