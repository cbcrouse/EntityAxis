using EntityAxis.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EntityAxis.Registration;

/// <summary>
/// Provides extensions for registering generic EntityAxis services using dependency injection.
/// </summary>
public static class EntityServiceRegistrationExtensions
{
    /// <summary>
    /// Registers a command service implementation along with its related interfaces
    /// (<see cref="ICommandService{TEntity, TKey}"/>, <see cref="ICreate{TEntity, TKey}"/>,
    /// <see cref="IUpdate{TEntity, TKey}"/>, and <see cref="IDelete{TEntity, TKey}"/>).
    /// </summary>
    /// <typeparam name="TService">
    /// The command service abstraction that implements <see cref="ICommandService{TEntity, TKey}"/>.
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The concrete implementation type that provides command functionality.
    /// </typeparam>
    /// <typeparam name="TEntity">The entity type the service operates on.</typeparam>
    /// <typeparam name="TKey">The type of the application entity's primary key.</typeparam>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="lifetime">
    /// The service lifetime to use for all registered interfaces (default: <see cref="ServiceLifetime.Transient"/>).
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddEntityAxisCommandService<TService, TImplementation, TEntity, TKey>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TService : class, ICommandService<TEntity, TKey>
        where TImplementation : class, TService
        where TEntity : class, IEntityId<TKey>
    {
        services.TryAddWithLifetime<TService, TImplementation>(lifetime);
        services.TryAddWithLifetime<ICommandService<TEntity, TKey>, TImplementation>(lifetime);
        services.TryAddWithLifetime<ICreate<TEntity, TKey>, TImplementation>(lifetime);
        services.TryAddWithLifetime<IUpdate<TEntity, TKey>, TImplementation>(lifetime);
        services.TryAddWithLifetime<IDelete<TEntity, TKey>, TImplementation>(lifetime);
        return services;
    }

    /// <summary>
    /// Registers a query service implementation along with its related interfaces
    /// (<see cref="IQueryService{TEntity, TKey}"/>, <see cref="IGetById{TEntity, TKey}"/>,
    /// <see cref="IGetAll{TEntity,TKey}"/>, and <see cref="IGetPaged{TEntity, TKey}"/>).
    /// </summary>
    /// <typeparam name="TService">
    /// The query service abstraction that implements <see cref="IQueryService{TEntity, TKey}"/>.
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The concrete implementation type that provides query functionality.
    /// </typeparam>
    /// <typeparam name="TEntity">The entity type the service queries.</typeparam>
    /// <typeparam name="TKey">The type of the application entity's primary key.</typeparam>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="lifetime">
    /// The service lifetime to use for all registered interfaces (default: <see cref="ServiceLifetime.Transient"/>).
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddEntityAxisQueryService<TService, TImplementation, TEntity, TKey>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TService : class, IQueryService<TEntity, TKey>
        where TImplementation : class, TService
        where TEntity : class, IEntityId<TKey>
    {
        services.TryAddWithLifetime<TService, TImplementation>(lifetime);
        services.TryAddWithLifetime<IQueryService<TEntity, TKey>, TImplementation>(lifetime);
        services.TryAddWithLifetime<IGetById<TEntity, TKey>, TImplementation>(lifetime);
        services.TryAddWithLifetime<IGetAll<TEntity, TKey>, TImplementation>(lifetime);
        services.TryAddWithLifetime<IGetPaged<TEntity, TKey>, TImplementation>(lifetime);
        return services;
    }

    /// <summary>
    /// Scans the assembly containing <typeparamref name="TMarker"/> for implementations of 
    /// <see cref="ICommandService{TEntity, TKey}"/> and <see cref="IQueryService{TEntity, TKey}"/>,
    /// and registers them and their related custom interfaces using the specified lifetime.
    /// </summary>
    /// <typeparam name="TMarker">A type from the target assembly to use as a marker for scanning.</typeparam>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="lifetime">The service lifetime to use for all registered interfaces (default is Scoped).</param>
    public static IServiceCollection AddEntityAxisCommandAndQueryServicesFromAssembly<TMarker>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        return services.AddEntityAxisCommandAndQueryServicesFromAssemblies([typeof(TMarker).Assembly], lifetime);
    }

    /// <summary>
    /// Scans the specified assemblies for implementations of 
    /// <see cref="ICommandService{TEntity, TKey}"/> and <see cref="IQueryService{TEntity, TKey}"/>,
    /// and registers them and their related custom interfaces using the specified lifetime.
    /// </summary>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="assemblies">Assemblies to scan for services.</param>
    /// <param name="lifetime">The service lifetime to use for all registered interfaces (default is Scoped).</param>
    public static IServiceCollection AddEntityAxisCommandAndQueryServicesFromAssemblies(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        foreach (var assembly in assemblies)
        {
            var types = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var implementationType in types)
            {
                services.RegisterServicesFromType(implementationType, typeof(ICommandService<,>), lifetime);
                services.RegisterServicesFromType(implementationType, typeof(IQueryService<,>), lifetime);
            }
        }

        return services;
    }

    private static void RegisterServicesFromType(
        this IServiceCollection services,
        Type implementationType,
        Type openGenericServiceType,
        ServiceLifetime lifetime)
    {
        if (implementationType.IsAssignableFromGenericType(openGenericServiceType))
        {
            foreach (var serviceInterface in implementationType.GetInterfacesAssignableFromGenericType(openGenericServiceType))
            {
                services.RecursiveAdd(serviceInterface, implementationType, lifetime);
            }
        }
    }

    private static void RecursiveAdd(
        this IServiceCollection services,
        Type serviceInterface,
        Type implementationType,
        ServiceLifetime lifetime,
        HashSet<Type>? visited = null)
    {
        visited ??= new HashSet<Type>();

        if (!visited.Add(serviceInterface))
        {
            // Already visited this interface, prevent infinite loop
            return;
        }

        services.TryAdd(new ServiceDescriptor(serviceInterface, implementationType, lifetime));

        foreach (var parent in serviceInterface.GetInterfaces())
        {
            services.RecursiveAdd(parent, implementationType, lifetime, visited);
        }
    }

    private static bool IsAssignableFromGenericType(this Type type, Type genericType)
    {
        if (type.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        return type.BaseType != null && type.BaseType.IsAssignableFromGenericType(genericType);
    }

    private static IEnumerable<Type> GetInterfacesAssignableFromGenericType(this Type type, Type genericType)
    {
        return type.GetInterfaces().Where(it => it.IsAssignableFromGenericType(genericType));
    }

    private static void TryAddWithLifetime<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptor = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
        services.TryAdd(descriptor);
    }
}
