using FluentValidation;
using SampleApp.Application.Models;

namespace SampleApp.Application.Validators;

public class OrderCreateModelValidator : AbstractValidator<OrderCreateModel>
{
    public OrderCreateModelValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0);
    }
}
