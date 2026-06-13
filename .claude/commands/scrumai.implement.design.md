---
description: Take a specification and design the solution, plan implementation steps, and write a design document.
argument-hint: [feature folder, e.g. .scrumai/features/001-user-auth] (defaults to latest)
# allowed-tools: Read, Write, Glob, Grep, Agent
---

# /scrumai.implement.design — Phase ② Design

> Skeleton. Author the prompt body below. Goal: convert `spec.md` into a
> concrete `design.md` + an ordered, checkable `plan.md`.

## Inputs
- Active feature folder (arg, or most recent `.scrumai/features/<NNN>-<name>/`).
- `spec.md` from Phase ①.
- Shared conventions: load the `scrumai-conventions` skill.
- Project principles: `.claude/memory/constitution.md`.

## Steps (TODO: flesh out)
1. Read the spec; confirm it has no open clarifications.
2. Delegate to the `scrumai.architect` subagent to design the solution:
   - identify affected services/modules (admin/identity/notify/payment, shared, Angular),
   - respect DDD/SOLID + the modular `BaseModule` DI system,
   - call out data/contract/migration impacts and risks.
3. Define **test cases** derived from the spec's acceptance scenarios:
   - **Unit tests** — the cases to add (per service/component), each tied to a requirement.
   - **Manual tests** — the browser/UI steps a tester walks to confirm behavior (drives the
     `tools-playwright` flow in Phase ④); omit if the feature has no UI/web behavior.
4. Write `design.md` from `.claude/templates/design.md` (including the Test Cases section).
5. Decompose into `plan.md` from `.claude/templates/plan.md` — small, ordered, each
   independently committable.

## Output
- `.scrumai/features/<NNN>-<name>/design.md`
- `.scrumai/features/<NNN>-<name>/plan.md`
- Report readiness for `/scrumai.implement.start`.
