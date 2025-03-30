using EntityAxis.MediatR.Commands;
using SampleApp.Domain;

namespace SampleApp.Application.Models;

/// <summary>
/// Represents the model used to update a product.
/// </summary>
public class ProductUpdateModel : IUpdateCommandModel<Product, int>
{
    /// <summary>
    /// Gets or sets the ID of the product to update.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated product name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the updated product description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the updated product price.
    /// </summary>
    public decimal? Price { get; set; }
}
