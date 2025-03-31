using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Tests.TestClasses;
using EntityAxis.MediatR.Validators;
using FluentAssertions;

namespace EntityAxis.MediatR.Tests.Validators;

public class DeleteEntityValidatorTests
{
    [Fact]
    public void Validate_Should_Pass_When_Id_Is_Valid()
    {
        // Arrange
        var validator = new DeleteEntityValidator<TestEntity, int>();
        var command = new DeleteEntityCommand<TestEntity, int>(123);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Should_Fail_When_Id_Is_Default()
    {
        // Arrange
        var validator = new DeleteEntityValidator<TestEntity, int>();
        var command = new DeleteEntityCommand<TestEntity, int>(0); // default int

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "Id" &&
            e.ErrorMessage == "An ID must be provided to delete an entity.");
    }
}
