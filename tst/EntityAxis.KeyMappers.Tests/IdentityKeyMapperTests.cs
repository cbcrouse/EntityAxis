using FluentAssertions;
using Xunit;

namespace EntityAxis.KeyMappers.Tests;

public class IdentityKeyMapperTests
{
    [Fact]
    public void ToDbKey_ShouldReturnSameValue()
    {
        // Arrange
        var mapper = new IdentityKeyMapper<int>();
        var input = 42;

        // Act
        var result = mapper.ToDbKey(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void ToAppKey_ShouldReturnSameValue()
    {
        // Arrange
        var mapper = new IdentityKeyMapper<int>();
        var input = 42;

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void ToDbKey_WithString_ShouldReturnSameValue()
    {
        // Arrange
        var mapper = new IdentityKeyMapper<string>();
        var input = "test";

        // Act
        var result = mapper.ToDbKey(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void ToAppKey_WithString_ShouldReturnSameValue()
    {
        // Arrange
        var mapper = new IdentityKeyMapper<string>();
        var input = "test";

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void ToDbKey_WithGuid_ShouldReturnSameValue()
    {
        // Arrange
        var mapper = new IdentityKeyMapper<Guid>();
        var input = Guid.NewGuid();

        // Act
        var result = mapper.ToDbKey(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void ToAppKey_WithGuid_ShouldReturnSameValue()
    {
        // Arrange
        var mapper = new IdentityKeyMapper<Guid>();
        var input = Guid.NewGuid();

        // Act
        var result = mapper.ToAppKey(input);

        // Assert
        result.Should().Be(input);
    }
} 