using EntityAxis.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApp.Persistence.Entities;

/// <summary>
/// Represents the database entity for an order.
/// </summary>
[Table("Orders")]
public class OrderDbEntity : IEntityId<Guid>
{
    /// <summary>
    /// The unique identifier for the order.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the customer who placed the order.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The total amount of the order.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The date the order was placed.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Optional notes about the order.
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
} 