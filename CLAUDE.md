# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Enterprise-grade multi-tenant SaaS microservices boilerplate. Backend is .NET 9 with DDD/SOLID patterns; frontend is Angular v21. Local orchestration uses .NET Aspire.

## Commands

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

### E2E Tests (Playwright, from `e2e/`)

```bash
npm ci && npx playwright install --with-deps
npx playwright test         # run all
npx playwright test --ui    # interactive UI mode
npx playwright test --debug
npx playwright show-report
```

### Local Infrastructure (Docker)

```bash
# Start SQL Server 2022 + MongoDB 8.0
docker compose -f deploy/dockercompose.local.infra.yml -p datntdev_microservices_infra up -d

# Stop and remove volumes
docker compose -f deploy/dockercompose.local.infra.yml -p datntdev_microservices_infra down -v
```

## Architecture

### Repository Layout

```
srcs/
  apps/
    Angular/          # Angular 21 SPA (Bootstrap, RxJS, OIDC client)
    Identity/         # OpenIddict identity provider UI
  infra/
    Aspire/           # .NET Aspire orchestrator (service discovery, local dev)
    Gateway/          # YARP API gateway (external routing → microservices)
    Migrator/         # EF Core & MongoDB migrations + seed data
  services/
    admin/            # Admin bounded context (SQL Server + MongoDB)
    identity/         # Core identity management (SQL Server, OpenIddict)
    notify/           # Notification service (scaffolded; not yet implemented)
    payment/          # Payment service (scaffolded; not yet implemented)
  shared/
    Application/      # FluentValidation, Mapster, EF Core base classes
    Common/           # Modular system, utilities
    Communication/    # HTTP proxy contracts for service-to-service calls
    Web.Host/         # Shared ASP.NET Core hosting configuration
tests/
  Common/             # MicroserviceBaseTest, TestAuthHandler, WebApplicationFactory base
  Srv.Admin/
  Srv.Identity/
  Srv.Notify/
  Srv.Payment/
```

### Service Layering

Each microservice follows a three-layer pattern:

- `Srv.<Name>.Contracts` — request/response DTOs
- `Srv.<Name>.Application` — domain logic, handlers, validators
- `Srv.<Name>.Web.Host` — ASP.NET Core entry point, DI wiring, middleware

### Modular DI System

Services register dependencies via `BaseModule` subclasses decorated with `[DependOn]`. Modules are discovered and loaded in dependency order at startup. When adding features to a service, create or extend a `BaseModule` rather than wiring directly in `Program.cs`.

### Service Communication

- **External clients → services:** YARP gateway routes HTTP traffic; external consumers never call services directly.
- **Service-to-service:** Synchronous only — generated HTTP proxies in `srcs/shared/Communication/` (TypeScript equivalents regenerated via `npm run nswag`). Communication is REST over HTTP; there is no gRPC.
- **Async messaging:** Not implemented. Kafka/Outbox-style eventing is a planned future addition, not present in the codebase today.

### Auth & Identity

OpenIddict provides OAuth2/OIDC. The `Identity` app hosts the login UI; the `Identity` service owns the user store. All other services validate JWTs. Integration tests use `TestAuthHandler` to bypass real token validation.

### Databases

Polyglot persistence: SQL Server (EF Core) is the default; the Admin service also supports MongoDB via `MongoDB.EntityFrameworkCore`. The `Migrator` project handles both.

### Observability

OpenTelemetry traces and metrics are emitted via the OTLP exporter (consumed by an external collector). The Aspire dashboard provides a local view during development. Note: no Prometheus/Grafana dashboards are wired up in the repo today.

## Testing Conventions

- Framework: MSTest with `EnableMSTestRunner=true`
- Integration tests inherit from `MicroserviceBaseTest` and use `WebApplicationFactory`
- Coverage output: `tests/TestResults/` (Cobertura format)
- CI generates an HTML coverage report via `reportgenerator`

## CI/CD

GitHub Actions workflows in `.github/workflows/`:
- `sub_unit_tests.yml` — build, EF migrations check, MSTest, Angular tests, coverage
- `sub_e2e_tests.yml` — Playwright tests with artifact upload
- `sub_security_scans.yml` / `schedule_security_scans.yml` — security scanning
- `sub_deploy.yml` / `manual_deploy.yml` — deployment pipeline
