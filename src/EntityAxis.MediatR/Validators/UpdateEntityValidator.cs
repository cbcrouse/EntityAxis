using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using FluentValidation;

namespace EntityAxis.MediatR.Validators;

/// <summary>
/// Validates an <see cref="UpdateEntityCommand{TModel, TEntity, TKey}"/> using a validator for the update model.
/// </summary>
public class UpdateEntityValidator<TModel, TEntity, TKey> : AbstractValidator<UpdateEntityCommand<TModel, TEntity, TKey>>
    where TModel : IUpdateCommandModel<TEntity, TKey>
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEntityValidator{TModel, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="modelValidator">Validator for the update model.</param>
    public UpdateEntityValidator(IValidator<TModel>? modelValidator)
    {
        if (modelValidator is null)
        {
            RuleFor(x => x.UpdateModel)
                .Custom((_, context) =>
                {
                    context.AddFailure($"No validator registered for {typeof(TModel).Name}. Ensure it is registered using AddValidatorsFromAssembly.");
                });

            return;
        }

        RuleFor(x => x.UpdateModel).NotNull().SetValidator(modelValidator);
    }
}
