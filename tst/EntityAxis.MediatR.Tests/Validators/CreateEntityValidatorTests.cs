using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Tests.TestClasses;
using EntityAxis.MediatR.Validators;
using FluentAssertions;
using FluentValidation;

namespace EntityAxis.MediatR.Tests.Validators;

public class CreateEntityValidatorTests
{
    [Fact]
    public void Validate_Should_Return_NoErrors_When_Model_Is_Valid()
    {
        // Arrange
        var modelValidator = new InlineValidator<TestCreateModel>();
        modelValidator.RuleFor(x => x.Id).GreaterThan(0);

        var validator = new CreateEntityValidator<TestCreateModel, TestEntity, int>(modelValidator);
        var command = new CreateEntityCommand<TestCreateModel, TestEntity, int>(new TestCreateModel { Id = 1 });

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Should_Return_Error_When_Model_Is_Invalid()
    {
        // Arrange
        var modelValidator = new InlineValidator<TestCreateModel>();
        modelValidator.RuleFor(x => x.Id).GreaterThan(0);

        var validator = new CreateEntityValidator<TestCreateModel, TestEntity, int>(modelValidator);
        var command = new CreateEntityCommand<TestCreateModel, TestEntity, int>(new TestCreateModel { Id = 0 });

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "CreateModel.Id");
    }

    [Fact]
    public void Validate_Should_Return_Error_When_No_ModelValidator_Provided()
    {
        // Arrange
        var validator = new CreateEntityValidator<TestCreateModel, TestEntity, int>(null);
        var command = new CreateEntityCommand<TestCreateModel, TestEntity, int>(new TestCreateModel());

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.ErrorMessage.Contains($"No validator registered for {nameof(TestCreateModel)}"));
    }
}
