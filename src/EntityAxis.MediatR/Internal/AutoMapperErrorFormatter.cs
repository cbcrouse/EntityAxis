using AutoMapper;
using System.Text;

namespace EntityAxis.MediatR.Internal;

/// <summary>
/// Helper class for formatting AutoMapper mapping errors for generic entity handlers.
/// </summary>
internal static class AutoMapperErrorFormatter
{
    /// <summary>
    /// Builds a detailed error message when a mapping from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/> fails.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="ex">The original AutoMapper mapping exception.</param>
    /// <returns>A detailed and developer-friendly error message.</returns>
    public static string Format<TSource, TDestination>(AutoMapperMappingException ex)
    {
        var fromType = typeof(TSource).FullName;
        var toType = typeof(TDestination).FullName;

        var sb = new StringBuilder();
        sb.AppendLine("[AutoMapper Mapping Failure]");
        sb.AppendLine($"- From: {fromType}");
        sb.AppendLine($"- To:   {toType}");

        if (ex.Message.Contains("Missing type map configuration"))
        {
            sb.AppendLine();
            sb.AppendLine("A valid AutoMapper CreateMap is likely missing.");
            sb.AppendLine("Please add the following in your AutoMapper Profile:");
            sb.AppendLine($"  CreateMap<{typeof(TSource).Name}, {typeof(TDestination).Name}>();");
        }

        sb.AppendLine();
        sb.AppendLine("Original AutoMapper error:");
        sb.AppendLine(ex.Message);

        return sb.ToString();
    }
}
