using EntityAxis.Abstractions;

namespace EntityAxis.EntityFramework.Tests.TestClasses;

public class TestProduct : IEntityId<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
