# EntityAxis.Abstractions

**EntityAxis.Abstractions** provides a lightweight set of interfaces and contracts for implementing generic CRUD (Create, Read, Update, Delete) operations across your application layer in a clean and consistent way. It is designed to support Clean Architecture and CQRS principles without taking on any infrastructure or framework dependencies.

## ✨ Features

- Common interfaces for query and command operations:
  - `IGetById<TEntity, TKey>`
  - `ICreate<TEntity, TKey>`
  - `IUpdate<TEntity, TKey>`
  - `IDelete<TEntity, TKey>`
- Fully generic with support for typed entity identifiers
- Flexible enough to support both anemic and rich domain models
- .NET Standard 2.0 support for wide compatibility

---

## 📦 Installation

```bash
dotnet add package EntityAxis.Abstractions
```

---

## 🧱 Example Usage

```csharp
public class User : IEntityId<Guid>
{
    public Guid Id { get; set; }
    public string Email { get; set; }
}
```

---

Create an implementation of one of the interfaces:

```csharp
public class InMemoryUserStore : IGetById<User, Guid>
{
    private readonly Dictionary<Guid, User> _db = new();

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _db.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }
}
```

Then plug it into your application services or handler logic.

---

## 🔗 Related Packages

- [EntityAxis.MediatR](https://www.nuget.org/packages/EntityAxis.MediatR): Generic MediatR handlers for Create/Read/Update/Delete
- [EntityAxis.EntityFramework](https://www.nuget.org/packages/EntityAxis.EntityFramework): Drop-in base classes for EF Core integration
- [EntityAxis.Registration](https://www.nuget.org/packages/EntityAxis.Registration): Offers `IServiceCollection` extension methods to register handlers, validators, and mapping configuration easily in your DI container.

---

## 📜 License

This project is licensed under the [MIT License](https://github.com/cbcrouse/EntityAxis/blob/main/LICENSE).
