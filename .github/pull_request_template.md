## 📋 Summary

<!-- Explain what this PR does -->

## 📝 PR Title Requirements

Your PR title must follow this format:

```
<type>(<Scope>): <Subject>
```

✅ Example: `feat(EntityFramework): Added support for optimistic concurrency`
❌ Avoid lowercase scopes like `test`, `core`, or `utils`

**Rules:**
- **Type** must be one of: `feat`, `fix`, `chore`, `refactor`, `test`, `docs`, `style`, `perf`, `ci`, `build`, `revert`
- **Scope** is required and must be **Title Case** (e.g., `EntityFramework`, `Registration`)
- **Subject** must be **sentence case**, starting with a capital letter (e.g., “Added support for X”)

See `.github/workflows/semantic-pr.yml` for the full validation rules.

---

## ✅ Checklist

- [ ] PR title follows [Conventional Commits](https://www.conventionalcommits.org/) with required scope
- [ ] All tests pass
- [ ] Added or updated tests
- [ ] Updated documentation
- [ ] Related issues referenced (if applicable)

---

## 🔗 Related Issues

<!-- Example: Closes #123 -->
