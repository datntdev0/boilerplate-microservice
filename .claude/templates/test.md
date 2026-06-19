# Test Report: [FEATURE NAME]

**Feature**: `<name>` · **Date**: [DATE] · **Verdict**: PASS / FAIL

## Environment
- Branch / commit: `feat/<name>` @ `<sha>`
- App started via: [Aspire / `ng serve`] · URL(s): http://localhost:4200, https://localhost:7240
- Data state: [migrator run? seed applied?] · DB: [SQL Server / MongoDB]
- Account used: [seeded admin email] · Browser: [chromium/…]

## Code Review Findings

### 1. [Severity] [File:Line] - [STATUS: open/fixed]
Finding description: …

### 2. [Severity] [File:Line] - [STATUS: open/fixed]
Finding description: …

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


## Test Results
| # | Scenario (from spec) | Type | Input / data used | Result | Evidence |
|---|----------------------|------|-------------------|--------|----------|
| 1 | …                    | unit/manual | … | PASS/FAIL | ./evidence/… |

## Test Data (for later debugging)
> Capture enough to reproduce each case later.
- Accounts / credentials: …
- Request payloads / form inputs: …
- Created or seeded IDs (tenant / user / role / …): …
- Config / env values that affected the run: …
- Raw logs & artifacts: `./evidence/<file>` (console, network, screenshots, coverage)

## Commands Run
- `dotnet test --settings .runsettings` → [result · log path]
- `npm run test:ci` → [result · log path]
- manual (`playwright-cli` steps) → `./evidence/<…>`

## Follow-ups
- [ ] [new task back to design/implement, if any]
