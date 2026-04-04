# GitHub Copilot Instructions - Enterprise SaaS Microservice Boilerplate

This repository is a standardized, reusable boilerplate for launching enterprise-grade, multi-tenant SaaS products. Follow these instructions and architectural patterns when generating code.

## 🎯 Project Context
- **Goal:** Enable rapid initialization of production-ready SaaS projects.
- **Key Features:** Multi-tenancy (isolated data/config), SSO/RBAC (OAuth2/OpenID Connect), real-time notifications (WebSocket/Kafka), pluggable billing, and centralized observability.
- **Reference Docs:** Always refer to [docs/](../docs/) for high-level requirements and architecture details.

## 🛠️ Tech Stack & Frameworks
- **Orchestration:** .NET Aspire (manages lifecycle/configuration).
- **Backend:** .NET 8+ Web API (Minimal APIs preferred for microservices).
- **Frontend (Web):** Angular (SPA) and Blazor (SSR for Identity Provider).
- **Frontend Themes:** Shared SCSS styles built with Gulp and Story Book.
- **Identity:** Blazor SSR application using OpenIddict.
- **Gateway:** .NET YARP (Reverse proxy, routing, and auth enforcement).
- **Messaging:** Apache Kafka (Asynchronous event-driven communication).
- **Databases:** Relational (SQL Server/EF Core) and Document (MongoDB), In-memory caching (Redis).
- **CI/CD:** GitHub Actions with private runners for sensitive tasks (e.g., DB migrations).
- **Testing:** MSTest (Backend), Vitest (Frontend), Playwright (E2E).

## 🏗️ Architectural Principles
- **Architecture Design:** [docs/2.architecture/](../docs/2.architecture/) provides detailed architectural patterns and design decisions. Always align generated code with these principles.
- **Multi-tenancy:** Ensure every request context is enriched with a `TenantId`. Data isolation must be enforced at the persistence layer.
- **Polyglot Persistence:** Choose the right tool for the job (SQL Server for ACID/Auth, MongoDB for high-volume logs/notifications).
- **Domain-Driven Design:** Each microservice should encapsulate a specific business domain (e.g., Identity, Billing) with clear boundaries.
- **Event-Driven:** Favor asynchronous communication via Kafka for inter-service updates to ensure decoupling.
- **API Gateway (YARP):** Centralized routing, rate limiting, and RBAC enforcement happen here.
- **Shared Libraries:** Common logic (logging, multi-tenancy middleware, service-to-service communication) should reside in [srcs/shared/](../srcs/shared/).

## 📁 Repository Structure
- Prefer using single Visual Studio solution for the whole repository to simplify dependency management.
- The Visual Studio project names must have prefix `datntdev.Microservice.` followed by the component name.
- [srcs/apps/](../srcs/apps/): Frontend clients and Blazor Identity Provider.
- [srcs/infra/](../srcs/infra/): Infrastructure components like Gateway, Migrator, and Aspire Orchestrator.
- [srcs/services/](../srcs/services/): Domain-specific microservices (Admin, Identity, Notify, Payment).
- [srcs/shared/](../srcs/shared/): Reusable backend libraries.
- [tests/](../tests/): All testing suites.

## 📝 Coding Standards
- **General:** Follow SOLID principles, ensure high cohesion and low coupling, follow Clean Architecture or Vertical Slice patterns.
- **C#:** Use file-scoped namespaces, var, primary constructors where applicable
- **Angular:** Use modular design, signals (if available/preferred), prefer use shared bundle CSS instead of isolated CSS in component.
- **Messaging:** Define event schemas clearly for Kafka producers/consumers.
- **Observability:** Ensure all services include health checks and structured logging for Grafana/OpenTelemetry.

## 🔐 Security & Compliance
- Follow OWASP Top 10.
- All sensitive data must be encrypted in transit and at rest.
- Use Secret Management for all connection strings and API keys.

## 💻 Development Processes
- **Application Services**: [app-service-design.md](../docs/2.architecture/03.app-service-design.md) guidelines for structuring application services and business logic.
- **Database Migration**: [database-migration.md](../docs/3.development/02.database-migration.md) instructions for managing database schema changes.
