namespace EntityAxis.KeyMappers;

/// <summary>
/// Defines a contract for mapping between application and database key types.
/// </summary>
/// <typeparam name="TAppKey">The type of the key in the application layer.</typeparam>
/// <typeparam name="TDbKey">The type of the key in the database layer.</typeparam>
public interface IKeyMapper<TAppKey, TDbKey>
{
    /// <summary>
    /// Converts an application key to a database key.
    /// </summary>
    /// <param name="appKey">The application key to convert.</param>
    /// <returns>The converted database key.</returns>
    TDbKey ToDbKey(TAppKey appKey);

    /// <summary>
    /// Converts a database key to an application key.
    /// </summary>
    /// <param name="dbKey">The database key to convert.</param>
    /// <returns>The converted application key.</returns>
    TAppKey ToAppKey(TDbKey dbKey);
}
