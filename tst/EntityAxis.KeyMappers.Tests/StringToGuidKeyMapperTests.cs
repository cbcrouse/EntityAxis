using FluentAssertions;
using Xunit;

namespace EntityAxis.KeyMappers.Tests;

public class StringToGuidKeyMapperTests
{
    [Fact]
    public void ToDbKey_WithValidGuidString_ShouldReturnGuid()
    {
        // Arrange
        var mapper = new StringToGuidKeyMapper();
        var input = "12345678-1234-1234-1234-123456789012";

        // Act
        var result = mapper.ToDbKey(input);

        // Assert
        result.Should().Be(Guid.Parse(input));
    }

    [Fact]
    public void ToDbKey_WithInvalidGuidString_ShouldThrowFormatException()
    {
        // Arrange
        var mapper = new StringToGuidKeyMapper();
        var input = "invalid-guid";

        // Act & Assert
        Assert.Throws<FormatException>(() => mapper.ToDbKey(input));
    }

    [Fact]
    public void ToAppKey_WithGuid_ShouldReturnString()
    {
        // Arrange
        var mapper = new StringToGuidKeyMapper();
        var input = Guid.NewGuid();

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be(input.ToString());
    }

    [Fact]
    public void ToAppKey_WithSpecificGuid_ShouldReturnCorrectString()
    {
        // Arrange
        var mapper = new StringToGuidKeyMapper();
        var input = new Guid("12345678-1234-1234-1234-123456789012");

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be("12345678-1234-1234-1234-123456789012");
    }
} 