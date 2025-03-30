# EntityAxis.Registration

**EntityAxis.Registration** provides helper extensions for registering EntityAxis services (query and command) in your dependency injection container. It works well with services built on top of [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions) and [EntityAxis.EntityFramework](https://www.nuget.org/packages/EntityAxis.EntityFramework).

## ✨ Features

- Register `ICommandService<TEntity, TKey>` and `IQueryService<TEntity, TKey>` with single-line extension methods
- Assembly scanning support to bulk register service implementations
- Built-in recursive interface registration for polymorphic usage

## 📦 Installation

```bash
dotnet add package EntityAxis.Registration
```

## 🧱 Example Usage

```csharp
services.AddCommandService<ICommandService<Product, int>, ProductCommandService, Product, int>();
services.AddQueryService<IQueryService<Product, int>, ProductQueryService, Product, int>();
```

To bulk register all services from an assembly:

```csharp
services.AddCommandAndQueryServicesFromAssembly<ProductCommandService>();
```

## 🔁 Interface Inheritance Support

The registration helpers automatically register base interfaces, including:

- `ICreate<TEntity, TKey>`
- `IUpdate<TEntity, TKey>`
- `IDelete<TEntity, TKey>`
- `IGetById<TEntity, TKey>`
- `IGetAll<TEntity, TKey>`
- `IGetPaged<TEntity, TKey>`

## 🔗 Related Packages

- [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions): Core CRUD service interfaces
- [EntityAxis.EntityFramework](https://www.nuget.org/packages/EntityAxis.EntityFramework): EF Core-based command/query service base classes
- [EntityAxis.MediatR](https://www.nuget.org/packages/EntityAxis.MediatR): Generic MediatR handlers for entity operations

---

## 📜 License

This project is licensed under the [MIT License](https://github.com/cbcrouse/EntityAxis/blob/main/LICENSE).
