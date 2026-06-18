# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Enterprise-grade multi-tenant SaaS microservices boilerplate. Backend is .NET 9 with DDD/SOLID patterns; frontend is Angular v21. Local orchestration uses .NET Aspire.

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

## Knowledge Base Maps

There is a curated knowledge base under `docs/`. Treat it as authoritative context that complements the code.

**Routing rule (applies to the main agent AND every subagent you spawn):**

1. Before starting any task, scan the request for the keywords in the table below.
2. If a keyword matches, **Read the mapped doc(s) first** and use them as primary context — they outline intended behavior, requirements, and conventions that the code alone may not reveal.
3. When you delegate work to a subagent (Agent/Task tool), **name the matching doc path in the subagent's prompt** (e.g. "Read `docs/2.requirements/03.authorization.md` first") so the routing survives delegation. Subagents inherit this CLAUDE.md, so they must apply this same rule recursively.
4. If several keywords match, read all mapped docs. If none match but the task is broad/architectural, start from the Overview docs.
5. Knowledge base docs describe intent and requirements; when they disagree with the code, trust the code for current behavior and flag the discrepancy.

| Keywords (match any) | Knowledge base doc |
| --- | --- |
| migration, EF Core, DbContext, schema change, seed data, squash migrations, Migrator | [Database Migrations](docs/3.development/database.migration.md) |
| authentication, login, sign-in, OAuth2, OIDC, OpenIddict, JWT, token, identity provider | [Authentication](docs/2.requirements/02.authentication.md) |
| authorization, permission, role, RBAC, access control, user access | [Authorization](docs/2.requirements/03.authorization.md) |
| tenant, multi-tenancy, multi-tenant, tenant isolation, tenant assignment | [Multi-Tenancy](docs/2.requirements/04.multi-tenancy.md) |
| notification, notify, email, alerts | [Notifications](docs/2.requirements/05.notifications.md) |
| payment, billing, subscription, invoice, checkout | [Payments](docs/2.requirements/06.payments.md) |
| platform requirement, NFR, non-functional, constraints, SLA | [Platform Requirements](docs/2.requirements/01.platform.md) |
| architecture, service layering, design pattern, bounded context, repository structure | [Architecture](docs/1.overview/02.architecture.md) |
| setup, getting started, run locally, build, test command, Aspire dashboard, prerequisites | [Getting Started](docs/1.overview/03.getting-started.md) |
| glossary, terminology, definition, what does X mean | [Glossary](docs/1.overview/99.glossary.md) |

Doc index: `docs/1.overview/` (intro, architecture, getting started, glossary), `docs/2.requirements/` (per-domain requirements), `docs/3.development/` (developer guides). Diagrams live in `docs/9.attachments/`.