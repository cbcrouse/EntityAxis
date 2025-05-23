﻿using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using FluentValidation;
using System.Collections.Generic;

namespace EntityAxis.MediatR.Validators;

/// <summary>
/// Validates a <see cref="DeleteEntityCommand{TEntity, TKey}"/> to ensure the identifier is present.
/// </summary>
public class DeleteEntityValidator<TEntity, TKey> : AbstractValidator<DeleteEntityCommand<TEntity, TKey>>
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteEntityValidator{TEntity, TKey}"/> class.
    /// </summary>
    public DeleteEntityValidator()
    {
        RuleFor(x => x.Id)
            .Must(id => !EqualityComparer<TKey>.Default.Equals(id, default!))
            .WithMessage("An ID must be provided to delete an entity.");
    }
}
