# EntityAxis.EntityFramework

**EntityAxis.EntityFramework** provides base classes to implement generic CRUD operations using [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions) with Entity Framework Core.

## ✨ Features

- Base class `EntityFrameworkServiceBase` to implement:
  - `ICommandService<TEntity, TKey>`
  - `IQueryService<TEntity, TKey>`
- AutoMapper support for converting between domain and database entities
- Full async support
- Easily extensible for custom behavior

## 📦 Installation

```bash
dotnet add package EntityAxis.EntityFramework
```

## 🧱 Example Usage

```csharp
public class Product : IEntityId<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class ProductDbEntity : IEntityId<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class ProductCommandService : EntityFrameworkCommandService<Product, ProductDbEntity, MyDbContext, int>
{
    public ProductCommandService(IDbContextFactory<MyDbContext> contextFactory, IMapper mapper)
        : base(contextFactory, mapper) { }
}

public class ProductQueryService : EntityFrameworkQueryService<Product, ProductDbEntity, MyDbContext, int>
{
    public ProductQueryService(IDbContextFactory<MyDbContext> contextFactory, IMapper mapper)
        : base(contextFactory, mapper) { }
}
```

## 🧪 AutoMapper Configuration

Make sure to register mappings between your domain entities and database entities:

```csharp
CreateMap<Product, ProductDbEntity>().ReverseMap();
```

## 🔗 Related Packages

- [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions): Defines common CRUD interfaces
- [EntityAxis.MediatR](https://www.nuget.org/packages/EntityAxis.MediatR): Generic MediatR handlers for Create/Update/Delete/Get
- [EntityAxis.Registration](https://www.nuget.org/packages/EntityAxis.Registration): Helper extensions to register services in your DI container

---

## 📜 License

This project is licensed under the [MIT License](https://github.com/cbcrouse/EntityAxis/blob/main/LICENSE).
