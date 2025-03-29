using AutoMapper;
using AutoMapper.Internal;

namespace EntityAxis.MediatR.Registration;

/// <summary>
/// Provides helpers for validating AutoMapper type mappings at startup.
/// </summary>
internal static class TypeMapValidationHelper
{
    /// <summary>
    /// Ensures that an AutoMapper mapping exists from the given source type to the destination type.
    /// </summary>
    /// <typeparam name="TSource">The source type to map from.</typeparam>
    /// <typeparam name="TDestination">The destination type to map to.</typeparam>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a mapping from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/> is not configured.
    /// </exception>
    public static void EnsureMappingExists<TSource, TDestination>(IMapper mapper)
    {
        var configuration = (MapperConfiguration)mapper.ConfigurationProvider;
        var typeMaps = configuration.Internal().GetAllTypeMaps();

        bool hasMapping = typeMaps.Any(m =>
            m.SourceType == typeof(TSource) &&
            m.DestinationType == typeof(TDestination));

        if (!hasMapping)
        {
            throw new InvalidOperationException(
                $"Missing AutoMapper mapping from {typeof(TSource).Name} to {typeof(TDestination).Name}. " +
                $"Ensure CreateMap<{typeof(TSource).Name}, {typeof(TDestination).Name}>() is defined in your mapping profile.");
        }
    }
}