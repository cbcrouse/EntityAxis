namespace EntityAxis.Abstractions;

/// <summary>
/// Defines a contract for entities that expose a unique identifier.
/// </summary>
/// <typeparam name="TIdentifierType">The type of the unique identifier.</typeparam>
public interface IEntityId<TIdentifierType>
{
    /// <summary>
    /// The unique identifier for the entity.
    /// </summary>
    TIdentifierType Id { get; set; }
}