# EntityAxis

**EntityAxis** is a set of composable, opinionated building blocks for clean, generic access to domain entities in .NET applications.

Built for teams embracing **Clean Architecture**, **CQRS**, and **MediatR**, EntityAxis helps reduce boilerplate and enforce consistency when implementing read/write flows backed by EF Core or any custom persistence layer.

---

## 📦 Packages

> 📌 Note: Packages are in early development and may change.

| Package | Description |
|--------|-------------|
| `EntityAxis.Abstractions` | Core interfaces for generic query and command services (IQueryEntityService, ICommandEntityService, etc.) |
| `EntityAxis.MediatR` | Generic MediatR request/response patterns with built-in validators |
| `EntityAxis.MediatR.Registration` | Service registration helpers for wiring up handlers and validators |
| `EntityAxis.EntityFramework` | Base EF Core data access implementations for generic entity operations |

---

## 🚀 Goals

- ✅ Reduce repetitive CRUD boilerplate
- ✅ Preserve Clean Architecture principles
- ✅ Support MediatR and FluentValidation out of the box
- ✅ Easily extendable to MongoDB, Dapper, or other stores
- ✅ Developer-friendly abstractions that work across many domains

---

## 🧪 Status

EntityAxis is currently in early development and **not yet published** to NuGet.  
We're working on stabilizing the abstractions and adding samples and documentation.

Stay tuned!

---

## 📁 Project Structure

```plaintext
/EntityAxis
├── src/
│   ├── EntityAxis.Abstractions/
│   ├── EntityAxis.MediatR/
│   ├── EntityAxis.MediatR.Registration/
│   ├── EntityAxis.EntityFramework/
├── samples/
│   └── CleanArchitectureSample/
├── README.md
```

---

## 📣 Contributing

Contributions will be welcome once the first public release is available.
Until then, feel free to watch the repo or open issues for ideas, bugs, or feedback.

---

## 📜 License

This project is licensed under the [MIT License](/LICENSE).
