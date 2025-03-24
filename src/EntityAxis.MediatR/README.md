# EntityAxis.MediatR

**EntityAxis.MediatR** provides fully generic MediatR command and query handlers that work out of the box with the interfaces defined in [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions). It is designed for use in Clean Architecture applications that already use [MediatR](https://github.com/jbogard/MediatR) and [AutoMapper](https://automapper.org/).

## ✨ Features

- Generic MediatR command/query handlers for:
  - `CreateEntityCommand`
  - `UpdateEntityCommand`
  - `DeleteEntityCommand`
  - `GetEntityByIdQuery`
  - `GetAllEntitiesQuery`
  - `GetPagedEntitiesQuery`
- AutoMapper support for mapping between models and entities
- FluentValidation integration with runtime safeguards and friendly errors
- Supports `TKey` flexibility: string, Guid, int, etc.

## 📦 Installation

```bash
dotnet add package EntityAxis.MediatR
```

## 🧱 Example Usage

```csharp
public class CreateUserModel
{
    public string Email { get; set; }
}

public class User : IEntityId<Guid>
{
    public Guid Id { get; set; }
    public string Email { get; set; }
}

// Configure AutoMapper
CreateMap<CreateUserModel, User>();

// MediatR Request
var id = await mediator.Send(new CreateEntityCommand<CreateUserModel, User, Guid>(model));
```

> ✅ Be sure to register your AutoMapper mappings, and register handlers using AddEntityAxisHandlers() from the [EntityAxis.Registration](https://www.nuget.org/packages/EntityAxis.Registration) package (recommended for DI setup).

## 🧪 Validation Integration

All handlers are paired with generic FluentValidation validators that:

- Validate required input (e.g., `Id`, paging params)
- Compose with your own model validators (e.g., `CreateUserModelValidator`)
- Throw clear and early errors if a validator is missing

To validate custom fields (e.g., email formatting), define a validator like:

```csharp
public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
{
    public CreateUserModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
```

This will be automatically invoked by the `CreateEntityValidator<TModel, TEntity, TKey>`.

---

## 🧠 Why Use EntityAxis

- Consistent patterns across your app
- Eliminate repetitive CRUD boilerplate
- Stay aligned with CQRS and Clean Architecture principles
- Focus on behavior and intent, not infrastructure

## 🔗 Related Packages

- [EntityAxis.Abstractions](https://www.nuget.org/packages/EntityAxis.Abstractions): Defines common CRUD contracts (IGetById, ICreate, etc.) and domain-friendly interfaces for clean application layers.
- [EntityAxis.EntityFramework](https://www.nuget.org/packages/EntityAxis.EntityFramework): Drop-in base classes for EF Core integration
- [EntityAxis.Registration](https://www.nuget.org/packages/EntityAxis.Registration): Offers `IServiceCollection` extension methods to register handlers, validators, and mapping configuration easily in your DI container.

---

## 📜 License

This project is licensed under the [MIT License](https://github.com/cbcrouse/EntityAxis/blob/main/LICENSE).