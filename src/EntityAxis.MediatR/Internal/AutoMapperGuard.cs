using System;
using AutoMapper;

namespace EntityAxis.MediatR.Internal;

/// <summary>
/// Provides helper methods to validate AutoMapper mappings at runtime.
/// </summary>
internal static class AutoMapperGuard
{
    /// <summary>
    /// Ensures that a mapping exists between the source and destination types.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="mapper">The AutoMapper instance to check.</param>
    /// <exception cref="InvalidOperationException">Thrown when no mapping is defined or the mapping fails.</exception>
    public static void EnsureMappingExists<TSource, TDestination>(IMapper mapper)
    {
        try
        {
            // Try to map from an empty source instance (assumes default ctor)
            var sourceInstance = Activator.CreateInstance<TSource>()!;
            mapper.Map<TDestination>(sourceInstance);
        }
        catch (AutoMapperMappingException)
        {
            throw new InvalidOperationException(
                $"Missing AutoMapper mapping from {typeof(TSource).Name} to {typeof(TDestination).Name}. " +
                $"Please define it in a profile using CreateMap<{typeof(TSource).Name}, {typeof(TDestination).Name}>().");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to validate AutoMapper mapping from {typeof(TSource).Name} to {typeof(TDestination).Name}. " +
                $"Ensure the model has a public parameterless constructor and that the mapping exists.",
                ex);
        }
    }
}