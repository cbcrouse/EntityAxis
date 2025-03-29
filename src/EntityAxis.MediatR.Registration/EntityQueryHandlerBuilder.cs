using EntityAxis.Abstractions;
using EntityAxis.MediatR.Handlers;
using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace EntityAxis.MediatR.Registration;

/// <summary>
/// Provides a fluent API for registering specific query-related MediatR handlers for an entity.
///
/// <para><b>Note:</b> This type is publicly exposed to support the <c>AddEntityQueryHandlers</c> extension method
/// but is not intended for direct use. Use the fluent extension method for registration instead.</para>
/// </summary>
/// <typeparam name="TEntity">The entity type the handlers are for.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class EntityQueryHandlerBuilder<TEntity, TKey>
    where TEntity : class, IEntityId<TKey>
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityQueryHandlerBuilder{TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="services">The service collection used to register handlers and validators.</param>
    public EntityQueryHandlerBuilder(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Registers the <see cref="GetEntityByIdHandler{TEntity, TKey}"/> and its corresponding validator.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityQueryHandlerBuilder<TEntity, TKey> AddGetById()
    {
        _services.AddTransient<IRequestHandler<GetEntityByIdQuery<TEntity, TKey>, TEntity?>, GetEntityByIdHandler<TEntity, TKey>>();
        _services.AddTransient<IValidator<GetEntityByIdQuery<TEntity, TKey>>, GetEntityByIdValidator<TEntity, TKey>>();

        return this;
    }

    /// <summary>
    /// Registers the <see cref="GetAllEntitiesHandler{TEntity, TKey}"/> query handler.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityQueryHandlerBuilder<TEntity, TKey> AddGetAll()
    {
        _services.AddTransient<IRequestHandler<GetAllEntitiesQuery<TEntity, TKey>, List<TEntity>>, GetAllEntitiesHandler<TEntity, TKey>>();

        return this;
    }

    /// <summary>
    /// Registers the <see cref="GetPagedEntitiesHandler{TEntity, TKey}"/> query handler and its validator.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityQueryHandlerBuilder<TEntity, TKey> AddGetPaged()
    {
        _services.AddTransient<IRequestHandler<GetPagedEntitiesQuery<TEntity, TKey>, PagedResult<TEntity>>, GetPagedEntitiesHandler<TEntity, TKey>>();
        _services.AddTransient<IValidator<GetPagedEntitiesQuery<TEntity, TKey>>, GetPagedEntitiesValidator<TEntity, TKey>>();

        return this;
    }

    /// <summary>
    /// Registers all supported query handlers for the entity: GetById, GetAll, and GetPaged.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public EntityQueryHandlerBuilder<TEntity, TKey> AddAllQueries()
    {
        return AddGetById()
            .AddGetAll()
            .AddGetPaged();
    }
}
