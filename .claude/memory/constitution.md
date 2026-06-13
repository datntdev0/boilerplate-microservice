# Project Constitution (skeleton)

> Principles the scrumai.reviewer enforces. Author the binding rules below.

## Core Principles
1. DDD bounded contexts — keep service boundaries clean; no cross-context leakage.
2. SOLID — single responsibility per class; depend on abstractions.
3. Modular DI — register via `BaseModule` + `[DependOn]`, not directly in `Program.cs`.
4. Layering — Contracts / Application / Web.Host separation per service.
5. Service comms — external via YARP gateway; sync via generated proxies; async via Kafka + Outbox.
6. Security — validate JWTs; enforce multi-tenant isolation; never log secrets.
7. Tests — integration tests inherit `MicroserviceBaseTest`; meet coverage expectations.
8. Simple — prefer straightforward solutions; YAGNI; avoid over-engineering.

## Definition of Done (TODO)
- [ ] Spec acceptance scenarios satisfied
- [ ] Tests added and passing
- [ ] No new analyzer/sanitization warnings
