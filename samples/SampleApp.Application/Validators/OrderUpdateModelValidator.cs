using FluentValidation;
using SampleApp.Application.Models;

namespace SampleApp.Application.Validators;

public class OrderUpdateModelValidator : AbstractValidator<OrderUpdateModel>
{
    public OrderUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0);
    }
}
