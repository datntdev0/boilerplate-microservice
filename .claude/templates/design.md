# Design: [FEATURE NAME]

**Feature**: `<name>` · **Spec**: ./spec.md · **Status**: Draft

## Approach
[High-level solution and key decisions.]

## Affected Components
- Services: [admin / identity / notify / payment]
- Shared: [Application / Common / Communication / Web.Host]
- Frontend: [Angular areas]
- Modules: [BaseModule(s) to add/extend]

## Data & Contracts
- EF Core / MongoDB changes: …
- Migrations: …
- gRPC / HTTP proxy / nswag contract changes: …

## Auth & Tenancy
[Impact on OpenIddict/JWT, multi-tenant isolation.]

## Risks & Trade-offs
- [risk → mitigation]

## Test Cases

Derived from the spec's acceptance scenarios. Drives implementation (Phase ③) and verification (Phase ④).

### Unit tests
| # | Target (service/component) | Case | Requirement |
|---|----------------------------|------|-------------|
| U1 | … | [what it asserts] | [spec FR/scenario] |

### Manual tests (web/UI)
> Omit this section if the feature has no UI/web behavior.

| # | Scenario | Steps | Expected result |
|---|----------|-------|-----------------|
| M1 | … | open → … → … | [observable outcome] |
