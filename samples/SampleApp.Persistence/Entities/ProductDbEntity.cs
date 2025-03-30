using EntityAxis.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApp.Persistence.Entities;

/// <summary>
/// Represents the database entity for a product.
/// </summary>
[Table("Products")]
public class ProductDbEntity : IEntityId<int>
{
    /// <summary>
    /// The unique identifier for the product.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// The name of the product.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The price of the product.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Optional description of the product.
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
}