using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Tests.TestClasses;
using EntityAxis.MediatR.Validators;
using FluentAssertions;

namespace EntityAxis.MediatR.Tests.Validators;

public class GetPagedEntitiesValidatorTests
{
    [Fact]
    public void Validate_Should_Fail_When_Page_Is_Less_Than_One()
    {
        // Arrange
        var validator = new GetPagedEntitiesValidator<TestEntity, int>();
        var query = new GetPagedEntitiesQuery<TestEntity, int>(page: 0, pageSize: 10);

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "Page" &&
            e.ErrorMessage == "Page number must be 1 or greater.");
    }

    [Fact]
    public void Validate_Should_Fail_When_PageSize_Is_Less_Than_One()
    {
        // Arrange
        var validator = new GetPagedEntitiesValidator<TestEntity, int>();
        var query = new GetPagedEntitiesQuery<TestEntity, int>(page: 1, pageSize: 0);

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "PageSize" &&
            e.ErrorMessage == "Page size must be greater than zero.");
    }

    [Fact]
    public void Validate_Should_Pass_With_Valid_Parameters()
    {
        // Arrange
        var validator = new GetPagedEntitiesValidator<TestEntity, int>();
        var query = new GetPagedEntitiesQuery<TestEntity, int>(page: 2, pageSize: 5);

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
