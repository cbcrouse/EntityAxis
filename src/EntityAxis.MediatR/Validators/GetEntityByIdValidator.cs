using EntityAxis.Abstractions;
using EntityAxis.MediatR.Queries;
using FluentValidation;

namespace EntityAxis.MediatR.Validators;

/// <summary>
/// Validates a <see cref="GetEntityByIdQuery{TEntity, TKey}"/> to ensure an identifier is provided.
/// </summary>
public class GetEntityByIdValidator<TEntity, TKey> : AbstractValidator<GetEntityByIdQuery<TEntity, TKey>>
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetEntityByIdValidator{TEntity, TKey}"/> class.
    /// </summary>
    public GetEntityByIdValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("An ID must be provided to retrieve an entity.");
    }
}
