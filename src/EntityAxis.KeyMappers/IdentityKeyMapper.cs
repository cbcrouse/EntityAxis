using System;

namespace EntityAxis.KeyMappers;

/// <summary>
/// A default implementation of <see cref="IKeyMapper{TAppKey, TDbKey}"/> for when the application and database key types are the same.
/// This mapper performs no conversion and simply returns the input key.
/// </summary>
/// <typeparam name="T">The type of the key.</typeparam>
public class IdentityKeyMapper<T> : IKeyMapper<T, T>
{
    /// <inheritdoc />
    public T ToDbKey(T appKey)
    {
        if (appKey is null)
            throw new ArgumentNullException(nameof(appKey), "Application key cannot be null.");
        return appKey;
    }

    /// <inheritdoc />
    public T ToAppKey(T dbKey)
    {
        if (dbKey is null)
            throw new ArgumentNullException(nameof(dbKey), "Database key cannot be null.");
        return dbKey;
    }
}
