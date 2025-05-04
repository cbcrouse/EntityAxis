using EntityAxis.Abstractions;

namespace SampleApp.Domain;

/// <summary>
/// Represents an order in the application domain.
/// </summary>
public class Order : IEntityId<string>
{
    public string Id { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Notes { get; set; }
} 