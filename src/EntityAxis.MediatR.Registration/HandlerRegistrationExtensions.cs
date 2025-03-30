using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace EntityAxis.MediatR.Registration;

/// <summary>
/// Provides dependency injection extensions for registering generic EntityAxis MediatR handlers and validators.
/// </summary>
public static class HandlerRegistrationExtensions
{
    /// <summary>
    /// Registers all generic command handlers and validators for <typeparamref name="TEntity"/>:
    /// Create, Update, and Delete.
    /// </summary>
    /// <typeparam name="TCreateModel">The model used for creating the entity.</typeparam>
    /// <typeparam name="TUpdateModel">The model used for updating the entity.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The identifier type of the entity.</typeparam>
    /// <param name="services">The service collection for dependency injection.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddEntityAxisCommandHandlers<TCreateModel, TUpdateModel, TEntity, TKey>(
        this IServiceCollection services)
        where TCreateModel : class
        where TUpdateModel : class, IUpdateCommandModel<TEntity, TKey>
        where TEntity : class, IEntityId<TKey>
    {
        return services.AddEntityAxisCommandHandlers<TEntity, TKey>(builder => builder.AddAllCommands<TCreateModel, TUpdateModel>());
    }

    /// <summary>
    /// Registers one or more command handlers for the specified entity using a fluent builder API.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    /// <param name="services">The service collection to register with.</param>
    /// <param name="configure">A delegate to configure the command handler builder.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddEntityAxisCommandHandlers<TEntity, TKey>(
        this IServiceCollection services,
        Action<EntityCommandHandlerBuilder<TEntity, TKey>> configure)
        where TEntity : class, IEntityId<TKey>
    {
        var builder = new EntityCommandHandlerBuilder<TEntity, TKey>(services);
        configure(builder);
        return services;
    }

    /// <summary>
    /// Configures and registers query-related MediatR handlers for the specified entity using a fluent builder.
    /// </summary>
    /// <typeparam name="TEntity">The entity type the handlers apply to.</typeparam>
    /// <typeparam name="TKey">The identifier type of the entity.</typeparam>
    /// <param name="services">The service collection to add handlers to.</param>
    /// <param name="configure">A delegate to configure which handlers to add using a fluent API.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddEntityAxisQueryHandlers<TEntity, TKey>(
        this IServiceCollection services,
        Action<EntityQueryHandlerBuilder<TEntity, TKey>> configure)
        where TEntity : class, IEntityId<TKey>
    {
        var builder = new EntityQueryHandlerBuilder<TEntity, TKey>(services);
        configure(builder);
        return services;
    }

    /// <summary>
    /// Registers all generic query handlers and validators for <typeparamref name="TEntity"/>:
    /// GetById, GetAll, and GetPaged.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The identifier type of the entity.</typeparam>
    /// <param name="services">The service collection for dependency injection.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddEntityAxisQueryHandlers<TEntity, TKey>(this IServiceCollection services)
        where TEntity : class, IEntityId<TKey>
    {
        return services.AddEntityAxisQueryHandlers<TEntity, TKey>(builder => builder.AddAllQueries());
    }

    /// <summary>
    /// Registers all EntityAxis MediatR handlers and validators and performs mapping validation.
    /// </summary>
    /// <typeparam name="TCreateModel">The model type used to create the entity.</typeparam>
    /// <typeparam name="TUpdateModel">The model type used to update the entity.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The identifier type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddEntityAxisHandlers<TCreateModel, TUpdateModel, TEntity, TKey>(
        this IServiceCollection services)
        where TCreateModel : class
        where TUpdateModel : class, IUpdateCommandModel<TEntity, TKey>
        where TEntity : class, IEntityId<TKey>
    {
        return services
            .AddEntityAxisCommandHandlers<TCreateModel, TUpdateModel, TEntity, TKey>()
            .AddEntityAxisQueryHandlers<TEntity, TKey>();
    }
}
