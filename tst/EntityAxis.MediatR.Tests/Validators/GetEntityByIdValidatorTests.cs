using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Tests.TestClasses;
using EntityAxis.MediatR.Validators;
using FluentAssertions;

namespace EntityAxis.MediatR.Tests.Validators;

public class GetEntityByIdValidatorTests
{
    [Fact]
    public void Validate_Should_Fail_When_Id_Is_Default()
    {
        // Arrange
        var validator = new GetEntityByIdValidator<TestEntity, int>();
        var query = new GetEntityByIdQuery<TestEntity, int>(0); // default int

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "Id" &&
            e.ErrorMessage == "An ID must be provided to retrieve an entity.");
    }

    [Fact]
    public void Validate_Should_Pass_When_Id_Is_Valid()
    {
        // Arrange
        var validator = new GetEntityByIdValidator<TestEntity, int>();
        var query = new GetEntityByIdQuery<TestEntity, int>(42);

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}