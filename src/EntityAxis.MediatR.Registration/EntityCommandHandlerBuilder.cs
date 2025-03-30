using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace EntityAxis.MediatR.Registration;

/// <summary>
/// Provides a fluent API for registering specific command-related MediatR handlers for an entity.
///
/// <para><b>Note:</b> This type is publicly exposed to support the <c>AddEntityCommandHandlers</c> extension method
/// but is not intended for direct use. Use the fluent extension method for registration instead.</para>
/// </summary>
/// <typeparam name="TEntity">The entity type the handlers are for.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class EntityCommandHandlerBuilder<TEntity, TKey>
    where TEntity : class, IEntityId<TKey>
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityCommandHandlerBuilder{TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="services">The service collection used to register handlers and validators.</param>
    public EntityCommandHandlerBuilder(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Registers the <see cref="CreateEntityHandler{TModel, TEntity, TKey}"/> handler and validator.
    /// </summary>
    /// <typeparam name="TModel">The create model type.</typeparam>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityCommandHandlerBuilder<TEntity, TKey> AddCreate<TModel>()
        where TModel : class
    {
        _services.AddTransient<IRequestHandler<CreateEntityCommand<TModel, TEntity, TKey>, TKey>, CreateEntityHandler<TModel, TEntity, TKey>>();
        _services.AddTransient<IValidator<CreateEntityCommand<TModel, TEntity, TKey>>, CreateEntityValidator<TModel, TEntity, TKey>>();

        return this;
    }

    /// <summary>
    /// Registers the <see cref="UpdateEntityHandler{TModel, TEntity, TKey}"/> handler and validator.
    /// </summary>
    /// <typeparam name="TModel">The update model type.</typeparam>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityCommandHandlerBuilder<TEntity, TKey> AddUpdate<TModel>()
        where TModel : class, IUpdateCommandModel<TEntity, TKey>
    {
        _services.AddTransient<IRequestHandler<UpdateEntityCommand<TModel, TEntity, TKey>, TKey>, UpdateEntityHandler<TModel, TEntity, TKey>>();
        _services.AddTransient<IValidator<UpdateEntityCommand<TModel, TEntity, TKey>>, UpdateEntityValidator<TModel, TEntity, TKey>>();

        return this;
    }

    /// <summary>
    /// Registers the <see cref="DeleteEntityHandler{TEntity, TKey}"/> handler and validator.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityCommandHandlerBuilder<TEntity, TKey> AddDelete()
    {
        _services.AddTransient<IRequestHandler<DeleteEntityCommand<TEntity, TKey>>, DeleteEntityHandler<TEntity, TKey>>();
        _services.AddTransient<IValidator<DeleteEntityCommand<TEntity, TKey>>, DeleteEntityValidator<TEntity, TKey>>();

        return this;
    }

    /// <summary>
    /// Registers all supported command handlers for the entity: Create, Update, and Delete.
    /// </summary>
    /// <typeparam name="TCreateModel">The create model type.</typeparam>
    /// <typeparam name="TUpdateModel">The update model type.</typeparam>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityCommandHandlerBuilder<TEntity, TKey> AddAllCommands<TCreateModel, TUpdateModel>()
        where TCreateModel : class
        where TUpdateModel : class, IUpdateCommandModel<TEntity, TKey>
    {
        return AddCreate<TCreateModel>()
            .AddUpdate<TUpdateModel>()
            .AddDelete();
    }
}
