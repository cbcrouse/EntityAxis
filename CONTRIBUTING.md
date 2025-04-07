# Contributing to EntityAxis

Thank you for considering contributing to **EntityAxis**! We welcome pull requests that improve the library's capabilities, documentation, tests, or integration experience.

---

## ✨ How to Contribute

- Fork the repo and create your branch from `main`.
- Follow the [Conventional Commits](https://www.conventionalcommits.org/) specification.
- Use `squash and merge` when merging pull requests.
- If your PR affects the public API or behavior, please update or add relevant tests and documentation.

---

## 🧪 Development Setup

To build and validate all projects:

```bash
dotnet build ./src/EntityAxis.sln
```

To run tests:

```bash
dotnet test ./src/EntityAxis.sln
```

---

## 📝 Commit Message Guidelines

We follow [Conventional Commits](https://www.conventionalcommits.org/) to support automated changelogs and versioning.

**Commit messages must include a scope.**

**Format:**

```
<type>(<scope>): <subject>
```

- `type`: One of `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`, `ci`, `build`, `revert`, or `perf`
- `scope`: The area of the codebase affected (e.g., `EntityFramework`, `Registration`, `Validators`)
- `subject`: Use **past tense**, in sentence case. (e.g., “Fixed bug in X” not “fix bug in X”)

**Examples:**

```
feat(EntityFramework): Added support for optimistic concurrency
fix(Registration): Fixed handler registration edge case
docs(README): Updated badges and install instructions
test(MediatR): Added unit tests for CreateEntityHandler
```

> ✅ Scope is **required** for all commits.

---

## ✅ PR Requirements

- PR titles must follow conventional commit style.
- All code must build and tests must pass.
- Include unit or integration tests for new functionality.
- Update the documentation if your change affects usage.

---

## 📜 License

By contributing, you agree that your contributions will be licensed under the MIT License.
