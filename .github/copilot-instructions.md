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
- **Identity Provider:** Blazor SSR application using OpenIddict.
- **API Gateway:** .NET YARP (Reverse proxy, routing, and auth enforcement).
- **Messaging:** Apache Kafka (Asynchronous event-driven communication).
- **Databases:** Relational (SQL Server/EF Core) and Document (MongoDB), In-memory caching (Redis).
- **CI/CD:** GitHub Actions with private runners for sensitive tasks (e.g., DB migrations).
- **Testing:** MSTest (Backend), Vitest (Frontend), Playwright (E2E).

## 🏗️ Architectural Documents
- **Architecture Design:** [docs/2.architecture/](../docs/2.architecture/) provides detailed architectural patterns and design decisions. Always align generated code with these principles.
- **Multi-tenancy:** Ensure every request context is enriched with a `TenantId`. Data isolation must be enforced at the persistence layer.
- **Polyglot Persistence:** Choose the right tool for the job (SQL Server for ACID/Auth, MongoDB for high-volume logs/notifications).
- **Domain-Driven Design:** Each microservice should encapsulate a specific business domain (e.g., Identity, Billing) with clear boundaries.
- **Event-Driven:** Favor asynchronous communication via Kafka for inter-service updates to ensure decoupling.
- **Shared Libraries:** Common logic (logging, multi-tenancy middleware, service-to-service communication) should reside in [srcs/shared/](../srcs/shared/).

## 📁 Repository Structure

#### Documentation
- `docs/`: High-level architectural and design documents.
- `docs/1.requirements/`: Business and technical requirements.
- `docs/2.architecture/`: Architectural patterns and design decisions.
- `docs/3.development/`: Development guidelines and best practices.

#### Source Code
- `srcs/`: All source code for microservices, shared libraries, and frontend applications.
- `srcs/apps/angular/`: Angular SPA frontend.
- `srcs/apps/identity/`: Blazor SSR Identity Provider handles authentication, authorization.
- `srcs/services/admin`: Manages administrative tasks and system configurations.
- `srcs/services/identity/`: Manages user authentication, authorization, and profiles.
- `srcs/services/notify/`: Manages system notifications and alerts.
- `srcs/services/payment/`: Manages financial transactions and payment flows.
- `srcs/shared/Common/`: Shared libraries common interfaces, types, contracts,...
- `srcs/shared/Communication/`: Shared communication services: http clients, message brokers.
- `srcs/shared/Application/`: Shared application services, database, validations,...
- `srcs/shared/Web.Host/`: Shared web hosting utilities, middleware, registrars,...
- `srcs/infra/gateway/`: YARP API Gateway configuration and customizations.
- `srcs/infra/migrator`: Database migration tools and scripts.
- `srcs/infra/aspire`: .NET Aspire configuration and customizations.

#### Testing
- `tests/Common/`: Shared test utilities and helpers for MSTest.
- `tests/Srv.Admin`: Unit and integration tests for the Admin microservice.
- `tests/Srv.Identity`: Unit and integration tests for the Identity microservice.
- `tests/Srv.Notify`: Unit and integration tests for the Notify microservice.
- `tests/Srv.Payment`: Unit and integration tests for the Payment microservice.
- `e2e`: Playwright end-to-end tests for critical user flows.

## 💻 Development Processes
- **Application Services**: [app-service-design.md](../docs/2.architecture/03.app-service-design.md) guidelines for structuring application services and business logic.
- **Database Migration**: [database-migration.md](../docs/3.development/02.database-migration.md) instructions for managing database schema changes.
- **HttpClient Generation**: [nswag-generate-http-client.md](../docs/3.development/04.nswag-generate-http-client.md) guidelines to generate HTTP clients with NSwag for inter-communication.

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
