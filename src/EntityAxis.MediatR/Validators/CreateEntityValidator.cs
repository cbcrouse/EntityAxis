using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using FluentValidation;

namespace EntityAxis.MediatR.Validators;

/// <summary>
/// Validates a <see cref="CreateEntityCommand{TModel, TEntity, TKey}"/> by validating its model.
/// </summary>
/// <typeparam name="TModel">The create model type.</typeparam>
/// <typeparam name="TEntity">The entity type being created.</typeparam>
/// <typeparam name="TKey">The identifier type of the entity.</typeparam>
public class CreateEntityValidator<TModel, TEntity, TKey> : AbstractValidator<CreateEntityCommand<TModel, TEntity, TKey>>
    where TModel : class
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEntityValidator{TModel, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="modelValidator">Validator for the create model.</param>
    public CreateEntityValidator(IValidator<TModel>? modelValidator)
    {
        if (modelValidator is null)
        {
            RuleFor(x => x.CreateModel)
                .Custom((_, context) =>
                {
                    context.AddFailure($"No validator registered for {typeof(TModel).Name}. Ensure it is registered using AddValidatorsFromAssembly.");
                });

            return;
        }

        RuleFor(x => x.CreateModel).NotNull().SetValidator(modelValidator);
    }
}
