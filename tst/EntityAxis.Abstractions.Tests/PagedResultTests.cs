using FluentAssertions;

namespace EntityAxis.Abstractions.Tests;

public class PagedResultTests
{
    [Fact]
    public void Constructor_WithNullItems_ThrowsArgumentNullException()
    {
        var act = () => new PagedResult<string>(null!, 10, 1, 5);
        act.Should().Throw<ArgumentNullException>().WithParameterName("items");
    }

    [Fact]
    public void TotalPages_ReturnsCorrectValue_WhenEvenlyDivisible()
    {
        var result = new PagedResult<string>(new List<string>(), 20, 1, 5);
        result.TotalPages.Should().Be(4);
    }

    [Fact]
    public void TotalPages_RoundsUp_WhenNotEvenlyDivisible()
    {
        var result = new PagedResult<string>(new List<string>(), 23, 1, 5);
        result.TotalPages.Should().Be(5);
    }

    [Fact]
    public void Properties_AreAccessibleAfterConstruction()
    {
        var items = new List<string> { "A", "B" };
        var result = new PagedResult<string>(items, 10, 2, 5);

        result.Items.Should().BeSameAs(items);
        result.TotalItemCount.Should().Be(10);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(5);
    }
}