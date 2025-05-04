using System;

namespace EntityAxis.KeyMappers;

/// <summary>
/// A key mapper that converts between string application keys and Guid database keys.
/// </summary>
public class StringToGuidKeyMapper : IKeyMapper<string, Guid>
{
    /// <inheritdoc />
    public Guid ToDbKey(string appKey)
    {
        if (string.IsNullOrWhiteSpace(appKey))
            throw new ArgumentException("Application key cannot be null or whitespace.", nameof(appKey));

        if (!Guid.TryParse(appKey, out var guid))
            throw new FormatException($"'{appKey}' is not a valid GUID.");

        return guid;
    }

    /// <inheritdoc />
    public string ToAppKey(Guid dbKey) => dbKey.ToString();
}
