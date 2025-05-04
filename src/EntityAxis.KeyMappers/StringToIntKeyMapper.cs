using System;

namespace EntityAxis.KeyMappers;

/// <summary>
/// A key mapper that converts between string application keys and integer database keys.
/// </summary>
public class StringToIntKeyMapper : IKeyMapper<string, int>
{
    /// <inheritdoc />
    public int ToDbKey(string appKey)
    {
        if (string.IsNullOrWhiteSpace(appKey))
            throw new ArgumentException("Application key cannot be null or whitespace.", nameof(appKey));

        if (!int.TryParse(appKey, out var intKey))
            throw new FormatException($"'{appKey}' is not a valid integer.");

        return intKey;
    }

    /// <inheritdoc />
    public string ToAppKey(int dbKey) => dbKey.ToString();
}
