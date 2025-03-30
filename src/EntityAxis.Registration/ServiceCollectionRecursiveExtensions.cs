using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace EntityAxis.Registration;

/// <summary>
/// Provides extension methods for recursively registering service implementations and their interfaces in the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionRecursiveExtensions
{
    /// <summary>
    /// Recursively registers the specified <paramref name="implementationType"/> for the given <paramref name="interfaceType"/> and all of its inherited interfaces.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services with.</param>
    /// <param name="interfaceType">The interface type to start registration from.</param>
    /// <param name="implementationType">The implementation type that provides the service.</param>
    /// <param name="lifetime">The desired <see cref="ServiceLifetime"/> for the service.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    /// <exception cref="ArgumentException">Thrown if the implementation type does not implement the specified interface type.</exception>
    public static IServiceCollection RecursiveAdd(
        this IServiceCollection services,
        Type interfaceType,
        Type implementationType,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (interfaceType == null)
            throw new ArgumentNullException(nameof(interfaceType));
        if (implementationType == null)
            throw new ArgumentNullException(nameof(implementationType));

        if (!interfaceType.IsAssignableFrom(implementationType))
        {
            throw new ArgumentException($"{implementationType.Name} does not implement {interfaceType.Name}");
        }

        var alreadyRegistered = services.Any(d =>
            d.ServiceType == interfaceType && d.ImplementationType == implementationType);

        if (!alreadyRegistered)
        {
            services.TryAdd(new ServiceDescriptor(interfaceType, implementationType, lifetime));
        }

        foreach (var baseInterface in interfaceType.GetInterfaces())
        {
            services.RecursiveAdd(baseInterface, implementationType, lifetime);
        }

        return services;
    }
}
