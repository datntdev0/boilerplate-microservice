---
description: Perform local code review and manual/unit/web testing against the documents, capturing evidence.
argument-hint: [feature folder] (defaults to latest)
# allowed-tools: Read, Write, Glob, Grep, Bash, Agent
---

# /scrumai.implement.verify — Phase ④ Verify

> Skeleton. Author the prompt body below. Goal: review the implementation and test it
> against the spec/design, recording findings + evidence in `test.md`.

## Inputs
- Active feature folder (arg, or most recent `.scrumai/features/<NNN>-<name>/`).
- `spec.md`, `design.md`, `plan.md`, and the local diff.
- Shared conventions: load the `scrumai-conventions` skill.

## Steps (TODO: flesh out)
1. Code review — delegate to `scrumai.reviewer` (enforces `.claude/memory/constitution.md`):
   correctness vs spec, DDD/SOLID, modular DI, contract/migration safety.
2. Testing — delegate to `scrumai.tester`:
   - unit: `dotnet test --settings .runsettings`, `npm run test:ci`,
   - manual web/UI: **only if the feature changes UI or web behavior** — start the app, then
     interactively drive a browser using the `tools-playwright` skill to walk through each
     acceptance scenario (this is dev-time manual testing, not the `e2e/` regression suite),
   - capture logs, coverage, and screenshots into `evidence/`.
3. Write `test.md` from `.claude/templates/test.md`: findings, test
   results, evidence links, and a PASS/FAIL verdict.

## Output
- `.scrumai/features/<NNN>-<name>/test.md`
- `.scrumai/features/<NNN>-<name>/evidence/`
- Report verdict and any follow-up tasks.
