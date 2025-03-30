using EntityAxis.Abstractions;

namespace SampleApp.Domain;

/// <summary>
/// Represents the domain model for a product.
/// </summary>
public class Product : IEntityId<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}