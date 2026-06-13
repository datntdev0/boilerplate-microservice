---
description: Take a specification and design the solution, plan implementation steps, and write a design document.
argument-hint: feature folder, e.g. .scrumai/features/001-user-auth (defaults to latest)
allowed-tools: Read, Write, Glob, Grep, Agent
---

# /scrumai.implement.design — Phase ② Design

<goal>
Convert `spec.md` into a concrete `design.md` plus an ordered, checkable `plan.md`.
</goal>

<inputs>
- Active feature folder (arg, or most recent `.scrumai/features/<NNN>-<name>/`).
- `spec.md` from Phase ①.
- Shared conventions: load the `scrumai-conventions` skill.
- Project principles: `.claude/memory/constitution.md`.
</inputs>

<steps>
   <step order="1">
   Read the spec; confirm it has no open clarifications.
   </step>
   <step order="2">
   Delegate to the `scrumai.architect` subagent to design the solution:
   - identify affected services/modules (admin/identity/notify/payment, shared, Angular),
   - respect DDD/SOLID + the modular `BaseModule` DI system,
   - call out data/contract/migration impacts and risks.
   </step>
   <step order="3">
   Define **test cases** derived from the spec's acceptance scenarios:
   - **Unit tests** — the cases to add (per service/component), each tied to a requirement.
   - **Manual tests** — the browser/UI steps a tester walks to confirm behavior (drives the
     `tools-playwright` flow in Phase ④). If the feature has no UI/web behavior, record
     **"N/A — no UI/web behavior"** explicitly in `design.md`; never silently omit.
   </step>
   <step order="4">
   Write `design.md` from `.claude/templates/design.md` (including the Test Cases section).
   </step>
   <step order="5">
   Decompose into `plan.md` from `.claude/templates/plan.md` — small, ordered, each independently committable.
   </step>
</steps>

<postValidate>
- Tick the **② Design** items in `checklist.md`. Honor checklist gating (see `scrumai-conventions`):
- Manual test cases must be defined or explicitly marked "N/A — no UI/web behavior".
</postValidate>

<output>
- `.scrumai/features/<NNN>-<name>/design.md`
- `.scrumai/features/<NNN>-<name>/plan.md`
- `checklist.md` with ② items ticked
- Report readiness for `/scrumai.implement.start`.
</output>
