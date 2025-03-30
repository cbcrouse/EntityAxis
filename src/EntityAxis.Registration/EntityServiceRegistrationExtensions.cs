using EntityAxis.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EntityAxis.Registration;

/// <summary>
/// Provides extensions for registering generic EntityAxis services using dependency injection.
/// </summary>
public static class EntityServiceRegistrationExtensions
{
    /// <summary>
    /// Registers the specified command service implementation and all its related interfaces.
    /// </summary>
    /// <typeparam name="TService">The command service abstraction.</typeparam>
    /// <typeparam name="TImplementation">The implementation type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The identifier type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddCommandService<TService, TImplementation, TEntity, TKey>(
        this IServiceCollection services)
        where TService : class, ICommandService<TEntity, TKey>
        where TImplementation : class, TService
        where TEntity : class, IEntityId<TKey>
    {
        return services.RecursiveAdd(typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    /// Registers the specified query service implementation and all its related interfaces.
    /// </summary>
    /// <typeparam name="TService">The query service abstraction.</typeparam>
    /// <typeparam name="TImplementation">The implementation type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The identifier type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddQueryService<TService, TImplementation, TEntity, TKey>(
        this IServiceCollection services)
        where TService : class, IQueryService<TEntity, TKey>
        where TImplementation : class, TService
        where TEntity : class, IEntityId<TKey>
    {
        return services.RecursiveAdd(typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    /// Automatically registers all discovered command and query services in the assembly of <typeparamref name="TMarker"/>.
    /// </summary>
    /// <typeparam name="TMarker">A type from the target assembly.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="lifetime">The desired service lifetime (default: Scoped).</param>
    public static IServiceCollection AddCommandAndQueryServicesFromAssembly<TMarker>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.AddServicesFromAssembly<TMarker>(typeof(ICommandService<,>), lifetime);
        services.AddServicesFromAssembly<TMarker>(typeof(IQueryService<,>), lifetime);
        return services;
    }

    /// <summary>
    /// Recursively registers all services from the same assembly as <typeparamref name="TMarker"/> that implement the given generic interface.
    /// </summary>
    /// <typeparam name="TMarker">A type from the target assembly.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="genericInterfaceType">The open generic interface to match.</param>
    /// <param name="lifetime">The lifetime to use when registering services.</param>
    public static IServiceCollection AddServicesFromAssembly<TMarker>(
        this IServiceCollection services,
        Type genericInterfaceType,
        ServiceLifetime lifetime)
    {
        var assembly = typeof(TMarker).Assembly;
        var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract);

        foreach (var implType in types)
        {
            var matchingInterfaces = implType
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType)
                .ToList();

            foreach (Type? interfaceType in matchingInterfaces)
            {
                services.RecursiveAdd(interfaceType, implType, lifetime);
            }
        }

        return services;
    }
}
