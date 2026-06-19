---
name: scrumai-conventions
description: Shared conventions for the ScrumAI workflow — repo layout, build/test commands, commit format, and where feature documents live. Loaded by the scrumai.* commands and agents.
user-invocable: false
---

# ScrumAI Conventions (skeleton)

> Author the shared knowledge below. Keep it the single source of truth so the four commands and their agents stay consistent.

## Feature artifacts
- One folder per feature: `.scrumai/features/<name>/` — `<name>` is the feature working directory.
- Files: `spec.md`, `design.md`, `test.md`, `checklist.md`, `evidence/`.
- Do not deviate the structure or content of these templates.

## Checklist gating
- Every feature folder has a `checklist.md` (from `.claude/templates/checklist.md`) with per-phase gates.
- A phase is **complete only when all its required items are checked**. An agent MUST NOT advance to the next phase, hand off, or report "done" while any required item is unchecked.
- If a required item cannot be met, **refers to the memory/troubleshoots.md to find guidance**. An agent MUST try to resolve blockers at least three times with different approaches.
- If a required items is actually not achievable, **STOP and report what is blocking** — never skip a gate silently. Each phase ticks its own items as it satisfies them.

## Project commands: build, start, run

### Local Infrastructure (Docker)

```bash
# Start SQL Server 2022 + MongoDB 8.0
docker compose -f deploy/dockercompose.local.infra.yml -p datntdev_microservices_infra up -d
# Stop and remove volumes
docker compose -f deploy/dockercompose.local.infra.yml -p datntdev_microservices_infra down -v
```

### Backend (.NET 9)

```bash
# Build
dotnet build datntdev.Microservice.slnf
# Run all unit/integration tests (with coverage)
dotnet test --no-build --settings .runsettings
# Run tests for a single service
dotnet test tests/Srv.Admin/datntdev.Microservice.Tests.Srv.Admin.csproj --no-build
# Apply database migrations & seed data
dotnet run --project ./srcs/infra/Migrator/datntdev.Microservice.Infra.Migrator.csproj
# Start all services locally via Aspire
dotnet run --project ./srcs/infra/Aspire/datntdev.Microservice.Infra.Aspire.csproj
```

### Frontend (Angular v21, from `srcs/apps/Angular/`)

```bash
npm install
npm start               # dev server
ng build                # production build
npm run test:ci         # headless unit tests (CI)
npm test                # watch mode
npm run nswag           # regenerate TypeScript API proxies from backend contracts
npm run storybook:start # Storybook component explorer
```
