---
description: Take a specification and design the solution, plan implementation tasks, and write a design document.
argument-hint: feature folder, e.g. .scrumai/features/user-auth (defaults to latest)
allowed-tools: Read, Write, Glob, Grep, Agent
---

# /scrumai.implement.design — Phase ② Design

<goal>
Convert `spec.md` into a concrete `design.md`, and record ordered implementation tasks in
`checklist.md` under ③ Implement (no separate plan file).
</goal>

<inputs>
- Active feature folder (arg, or most recent `.scrumai/features/<name>/`).
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
   Record the implementation tasks in `checklist.md` under **③ Implement** — small, ordered, each
   independently committable, with a suggested conventional commit type. There is no separate `plan.md`.
   </step>
</steps>

<postValidate>
- Tick the **② Design** items in `checklist.md`. Honor checklist gating (see `scrumai-conventions`):
  manual test cases must be defined or explicitly marked "N/A — no UI/web behavior", and the ③ task
  list must be populated.
</postValidate>

<output>
- `.scrumai/features/<name>/design.md`
- `.scrumai/features/<name>/checklist.md` (③ tasks added, ② items ticked)
- Report readiness for `/scrumai.implement.start`.
</output>
