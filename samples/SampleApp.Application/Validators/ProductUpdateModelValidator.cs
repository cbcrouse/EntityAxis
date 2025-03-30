using FluentValidation;
using SampleApp.Application.Models;

namespace SampleApp.Application.Validators;

/// <summary>
/// Validates the <see cref="ProductUpdateModel"/> to ensure any provided fields
/// are valid and the entity ID is a positive value.
/// </summary>
public class ProductUpdateModelValidator : AbstractValidator<ProductUpdateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductUpdateModelValidator"/> class
    /// and defines validation rules for updating a product.
    /// </summary>
    public ProductUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().When(x => x.Name != null)
            .WithMessage("If provided, name must not be empty.");

        RuleFor(x => x.Description)
            .NotEmpty().When(x => x.Description != null)
            .WithMessage("If provided, description must not be empty.");

        RuleFor(x => x.Price)
            .GreaterThan(0).When(x => x.Price.HasValue)
            .WithMessage("If provided, price must be greater than 0.");
    }
}
