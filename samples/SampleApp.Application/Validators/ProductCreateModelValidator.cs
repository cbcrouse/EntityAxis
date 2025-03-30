using FluentValidation;
using SampleApp.Application.Models;

namespace SampleApp.Application.Validators;

/// <summary>
/// Validates the <see cref="ProductCreateModel"/> to ensure all required fields
/// are populated and conform to business rules.
/// </summary>
public class ProductCreateModelValidator : AbstractValidator<ProductCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCreateModelValidator"/> class
    /// and defines validation rules for creating a product.
    /// </summary>
    public ProductCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}
