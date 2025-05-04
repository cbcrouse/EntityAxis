using FluentAssertions;
using Xunit;

namespace EntityAxis.KeyMappers.Tests;

public class StringToIntKeyMapperTests
{
    [Fact]
    public void ToDbKey_WithValidIntString_ShouldReturnInt()
    {
        // Arrange
        var mapper = new StringToIntKeyMapper();
        var input = "42";

        // Act
        var result = mapper.ToDbKey(input);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ToDbKey_WithInvalidIntString_ShouldThrowFormatException()
    {
        // Arrange
        var mapper = new StringToIntKeyMapper();
        var input = "not-a-number";

        // Act & Assert
        Assert.Throws<FormatException>(() => mapper.ToDbKey(input));
    }

    [Fact]
    public void ToAppKey_WithInt_ShouldReturnString()
    {
        // Arrange
        var mapper = new StringToIntKeyMapper();
        var input = 42;

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be("42");
    }

    [Fact]
    public void ToAppKey_WithZero_ShouldReturnZeroString()
    {
        // Arrange
        var mapper = new StringToIntKeyMapper();
        var input = 0;

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be("0");
    }

    [Fact]
    public void ToAppKey_WithNegativeInt_ShouldReturnNegativeString()
    {
        // Arrange
        var mapper = new StringToIntKeyMapper();
        var input = -42;

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be("-42");
    }
} 