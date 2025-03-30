# EntityAxis.MediatR.Registration

**EntityAxis.MediatR.Registration** provides convenient `IServiceCollection` extension methods for registering generic MediatR handlers and validators from [EntityAxis.MediatR](https://www.nuget.org/packages/EntityAxis.MediatR).

## ✨ Features

- Fluent builder-based APIs for registering generic MediatR command and query handlers
- Optional AutoMapper validation during handler setup
- Isolated support for query-only, command-only, or full handler registration
- No boilerplate registration needed per entity

## 📦 Installation

```bash
dotnet add package EntityAxis.MediatR.Registration
```

## 🧱 Example Usage

```csharp
services.AddEntityAxisHandlers<ProductCreateModel, ProductUpdateModel, Product, int>();
```

Or, to register only query or command handlers:

```csharp
services.AddEntityQueryHandlers<Product, int>();
services.AddEntityCommandHandlers<ProductCreateModel, ProductUpdateModel, Product, int>();
```

For finer control:

```csharp
services.AddEntityQueryHandlers<Product, int>(builder => builder.AddGetAll());
services.AddEntityCommandHandlers<Product, int>(builder => builder.AddCreate<ProductCreateModel>());
```

## 🧪 FluentValidation Support

All generic handlers are registered alongside validators:

- `CreateEntityValidator<TModel, TEntity, TKey>`
- `UpdateEntityValidator<TModel, TEntity, TKey>`
- `DeleteEntityValidator<TEntity, TKey>`
- `GetEntityByIdValidator<TEntity, TKey>`
- `GetPagedEntitiesValidator<TEntity, TKey>`

Custom validators (e.g. `CreateProductModelValidator`) are automatically composed.

## 🔗 Related Packages

- [EntityAxis.MediatR](https://www.nuget.org/packages/EntityAxis.MediatR): Generic MediatR handlers and validators
- [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions): Core CRUD interfaces and contracts
- [EntityAxis.EntityFramework](https://www.nuget.org/packages/EntityAxis.EntityFramework): Base EF Core services for CRUD

---

## 📜 License

This project is licensed under the [MIT License](https://github.com/cbcrouse/EntityAxis/blob/main/LICENSE).
