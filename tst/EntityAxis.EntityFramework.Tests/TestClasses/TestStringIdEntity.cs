using EntityAxis.Abstractions;

namespace EntityAxis.EntityFramework.Tests.TestClasses;

/// <summary>
/// A test entity with a string ID.
/// </summary>
public class TestStringIdEntity : IEntityId<string>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
