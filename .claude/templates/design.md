# Design: [FEATURE NAME]

**Feature**: `<name>` · **Spec**: ./spec.md

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
| #   | Target (service/component) | Requirement        | Case              |
|-----|----------------------------|--------------------|-------------------|
| U01 | …                          | [spec FR/scenario] | [what it asserts] |

### Manual tests (web/UI)
> If the feature has no UI/web behavior, replace the table with: **N/A — no UI/web behavior**. Do not delete the section — Phase ④ checks it and the checklist gate requires an explicit answer.

| #   | Scenario | Steps                                   | Expected result      |
|-----|----------|-----------------------------------------|----------------------|
| M01 | …        | 1. step 1 <br> 2. step 2 <br> 3. step 3 | [observable outcome] |
