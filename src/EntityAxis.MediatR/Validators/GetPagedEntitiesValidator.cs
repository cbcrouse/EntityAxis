using EntityAxis.Abstractions;
using EntityAxis.MediatR.Queries;
using FluentValidation;

namespace EntityAxis.MediatR.Validators;

/// <summary>
/// Validates a <see cref="GetPagedEntitiesQuery{TEntity, TKey}"/> to ensure valid paging parameters.
/// </summary>
public class GetPagedEntitiesValidator<TEntity, TKey> : AbstractValidator<GetPagedEntitiesQuery<TEntity, TKey>>
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPagedEntitiesValidator{TEntity, TKey}"/> class.
    /// </summary>
    public GetPagedEntitiesValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be 1 or greater.");

        RuleFor(x => x.PageSize).GreaterThan(0)
            .WithMessage("Page size must be greater than zero.");
    }
}
