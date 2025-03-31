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
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
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
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
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
    /// and registers them along with their related interfaces using the specified lifetime.
    /// </summary>
    /// <typeparam name="TMarker">A type from the target assembly to use as a marker for scanning.</typeparam>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="lifetime">
    /// The service lifetime to use for all registered interfaces (default: <see cref="ServiceLifetime.Transient"/>).
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddEntityAxisCommandAndQueryServicesFromAssembly<TMarker>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        return services.AddEntityAxisCommandAndQueryServicesFromAssemblies([typeof(TMarker).Assembly], lifetime);
    }

    /// <summary>
    /// Scans the specified assemblies for implementations of
    /// <see cref="ICommandService{TEntity, TKey}"/> and <see cref="IQueryService{TEntity, TKey}"/>,
    /// and registers each implementation along with its related interfaces
    /// (<see cref="ICreate{TEntity, TKey}"/>, <see cref="IUpdate{TEntity, TKey}"/>,
    /// <see cref="IGetById{TEntity, TKey}"/>, etc.) using the specified lifetime.
    /// </summary>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="assemblies">The assemblies to scan for service implementations.</param>
    /// <param name="lifetime">
    /// The service lifetime to use for all registered interfaces (default: <see cref="ServiceLifetime.Transient"/>).
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddEntityAxisCommandAndQueryServicesFromAssemblies(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToList();

        RegisterServicesForInterface(
            services,
            types,
            typeof(ICommandService<,>),
            nameof(AddEntityAxisCommandService),
            lifetime);

        RegisterServicesForInterface(
            services,
            types,
            typeof(IQueryService<,>),
            nameof(AddEntityAxisQueryService),
            lifetime);

        return services;
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

    private static void RegisterServicesForInterface(
    IServiceCollection services,
    IReadOnlyList<Type> implementationTypes,
    Type targetGenericInterface,
    string registrationMethodName,
    ServiceLifetime lifetime)
    {
        foreach (Type implementationType in implementationTypes)
        {
            RegisterMatchingInterfaces(services, implementationType, targetGenericInterface, registrationMethodName, lifetime);
        }
    }

    private static void RegisterMatchingInterfaces(
        IServiceCollection services,
        Type implementationType,
        Type targetGenericInterface,
        string registrationMethodName,
        ServiceLifetime lifetime)
    {
        Type[] implementedInterfaces = implementationType.GetInterfaces();

        foreach (Type interfaceType in implementedInterfaces)
        {
            if (TryGetGenericMatch(interfaceType, targetGenericInterface, out Type entityType, out Type keyType))
            {
                RegisterGenericService(services, registrationMethodName, interfaceType, implementationType, entityType, keyType, lifetime);
                break; // Only register once per implementation
            }
        }
    }

    private static bool TryGetGenericMatch(
        Type interfaceType,
        Type targetGenericInterface,
        out Type entityType,
        out Type keyType)
    {
        IEnumerable<Type> inheritedAndSelfInterfaces = interfaceType.GetInterfaces().Append(interfaceType);

        foreach (Type candidateInterface in inheritedAndSelfInterfaces)
        {
            if (candidateInterface.IsGenericType &&
                candidateInterface.GetGenericTypeDefinition() == targetGenericInterface)
            {
                Type[] genericArguments = candidateInterface.GetGenericArguments();
                entityType = genericArguments[0];
                keyType = genericArguments[1];
                return true;
            }
        }

        entityType = null!;
        keyType = null!;
        return false;
    }

    private static void RegisterGenericService(
        IServiceCollection services,
        string registrationMethodName,
        Type interfaceType,
        Type implementationType,
        Type entityType,
        Type keyType,
        ServiceLifetime lifetime)
    {
        MethodInfo method = typeof(EntityServiceRegistrationExtensions)
            .GetMethod(registrationMethodName, BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(interfaceType, implementationType, entityType, keyType);

        method.Invoke(null, [services, lifetime]);
    }
}
