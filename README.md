# EntityAxis

**EntityAxis** is a modular, opinionated library for building clean, maintainable applications using **CQRS**, **MediatR**, and **Entity Framework Core** — with full support for your own abstractions and persistence strategies.

Built for teams embracing **Clean Architecture**, EntityAxis helps reduce boilerplate and enforce consistency in application flows, from command/query operations to handler validation and service registration.

---

## 📚 Documentation

Comprehensive documentation is available in the [GitHub Wiki](../../wiki):

- 🚀 [Getting Started](../../wiki/Getting-Started)
- 📐 [Architecture Guide](../../wiki/Architecture-Guide)
- ⚙️ [Command & Query Abstractions](../../wiki/Command-&-Query-Abstractions)
- 🗃️ [EF Core Integration](../../wiki/EF-Core-Integration)
- 📨 [MediatR Integration](../../wiki/MediatR-Integration)
- 🔧 [Extending the Library](../../wiki/Extending-the-Library)

Whether you're working in a monolith, microservice, or modular system, EntityAxis scales with your needs.

---

## 📦 Packages

> 📌 EntityAxis is under active development. APIs may still change before initial release.

| Package                              | Description                                                                 |
|--------------------------------------|-----------------------------------------------------------------------------|
| `EntityAxis.Abstractions`           | Core interfaces for command/query service abstractions                     |
| `EntityAxis.MediatR`                | Generic MediatR request/response and built-in validators                   |
| `EntityAxis.MediatR.Registration`   | Fluent registration helpers for handlers and validators                    |
| `EntityAxis.Registration`           | DI registration helpers for command/query services                         |
| `EntityAxis.EntityFramework`        | Base implementations for EF Core–backed entity operations                  |

---

## 🎯 Goals

- ✅ Eliminate repetitive CRUD logic using generic abstractions
- ✅ Align with Clean Architecture principles
- ✅ Integrate seamlessly with MediatR and FluentValidation
- ✅ Support extensibility via interfaces — not frameworks
- ✅ Enable alternative persistence models (Dapper, MongoDB, APIs, etc.)
- ✅ Make registration and validation easy and consistent

---

## 📁 Project Structure

```plaintext
/EntityAxis
├── src/
│   ├── EntityAxis.Abstractions/
│   ├── EntityAxis.MediatR/
│   ├── EntityAxis.MediatR.Registration/
│   ├── EntityAxis.Registration/
│   ├── EntityAxis.EntityFramework/
├── samples/
│   └── CleanArchitectureSample/
├── README.md
```

---

## 🧪 Project Status

EntityAxis is currently in pre-release.

We're stabilizing APIs and finalizing sample usage before publishing to NuGet.

Follow the repo to stay updated, or jump into the [wiki](../../wiki) to start experimenting with the APIs today.

---

## 🤝 Contributing

Contributions are welcome once the library reaches its first public milestone release.

Until then, feel free to:

- ⭐ Star the project
- 🐛 Open issues for bugs or feedback
- 📣 Watch the repository for updates

---

## 📜 License

This project is licensed under the [MIT License](/LICENSE).
