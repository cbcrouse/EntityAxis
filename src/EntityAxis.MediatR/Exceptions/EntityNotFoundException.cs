using System;

namespace EntityAxis.MediatR.Exceptions;

/// <summary>
/// Represents an error when an entity with the specified identifier could not be found.
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityName">The name of the entity type.</param>
    /// <param name="id">The identifier of the entity.</param>
    public EntityNotFoundException(string entityName, object? id)
        : base($"Unable to find {entityName} with ID \"{id}\".") { }
}
