# EntityAxis.KeyMappers

**EntityAxis.KeyMappers** provides key mapping implementations for EntityAxis, allowing seamless conversion between application and database key types.

## ✨ Features

- Generic key mappers for common scenarios:
  - `IdentityKeyMapper<T>` for when key types are the same
  - `StringToGuidKeyMapper` for string-to-Guid conversion
  - `StringToIntKeyMapper` for string-to-integer conversion
- Easy to extend with custom key mappers
- Stateless implementations for optimal performance

## 📦 Installation

```bash
dotnet add package EntityAxis.KeyMappers
```

## 🧱 Available Key Mappers

### IdentityKeyMapper<T>

A default implementation for when the application and database key types are the same. This mapper performs no conversion and simply returns the input key.

```csharp
// Register in DI container
services.AddSingleton<IKeyMapper<int, int>, IdentityKeyMapper<int>>();
```

### StringToGuidKeyMapper

Converts between string keys in the application layer and GUID keys in the database.

```csharp
// Register in DI container
services.AddSingleton<IKeyMapper<string, Guid>, StringToGuidKeyMapper>();
```

### StringToIntKeyMapper

Converts between string keys in the application layer and integer keys in the database.

```csharp
// Register in DI container
services.AddSingleton<IKeyMapper<string, int>, StringToIntKeyMapper>();
```

## 🧪 Creating Custom Key Mappers

You can create custom key mappers by implementing the `IKeyMapper<TAppKey, TDbKey>` interface:

```csharp
public class CustomKeyMapper : IKeyMapper<string, long>
{
    public long ToDbKey(string appKey) => long.Parse(appKey);
    public string ToAppKey(long dbKey) => dbKey.ToString();
}
```

## 🧠 Best Practices

- Register key mappers as singletons since they are stateless
- Ensure your key mappers handle edge cases (null values, invalid formats, etc.)
- Consider adding validation in your key mappers to catch conversion issues early
- Use the `IdentityKeyMapper` when your application and database key types are the same

## 🔗 Related Packages

- [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions): Defines the `IKeyMapper` interface and core contracts
- [EntityAxis.EntityFramework](https://www.nuget.org/packages/EntityAxis.EntityFramework): Uses key mappers for database operations
- [EntityAxis.MediatR](https://www.nuget.org/packages/EntityAxis.MediatR): Works with key mappers for command/query handling
- [EntityAxis.Registration](https://www.nuget.org/packages/EntityAxis.Registration): Provides extension methods for easy key mapper registration

## 📜 License

This project is licensed under the [MIT License](https://github.com/cbcrouse/EntityAxis/blob/main/LICENSE). 