# Test Report: [FEATURE NAME]

**Feature**: `<name>` · **Date**: [DATE] · **Verdict**: PASS / FAIL

## Environment
- Branch / commit: `feat/<name>` @ `<sha>`
- App started via: [Aspire / `ng serve`] · URL(s): http://localhost:4200, https://localhost:7240
- Data state: [migrator run? seed applied?] · DB: [SQL Server / MongoDB]
- Account used: [seeded admin email] · Browser: [chromium/…]

## Code Review Findings
| # | Severity | File:Line | Finding | Status |
|---|----------|-----------|---------|--------|
| 1 | …        | …         | …       | open/fixed |

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
