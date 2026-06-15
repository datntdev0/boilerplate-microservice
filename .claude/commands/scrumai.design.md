---
name: scrumai.design
description: Take a specification and design the solution, plan implementation tasks, and write a design document.
argument-hint: feature folder, e.g. .scrumai/features/user-auth (defaults to latest)
allowed-tools: Read, Write, Glob, Grep, Agent
---

# /scrumai.design — Phase ② Design

<goal>
Convert `spec.md` into a concrete `design.md`, and record ordered implementation tasks in `checklist.md` under ③ Implement (no separate plan file).
</goal>

<inputs>
- `$DIRECTORY`: resolve the feature directory name provided by the human. E.g: `.scrumai/features/<name>/`.
- Shared conventions: load the `scrumai-conventions` skill.
</inputs>

<steps>
  <step order="1">
  - Resolve the `$DIRECTORY` and check the `${DIRECTORY}/spec.md` exists
  - If the directory or spec is missing, report and halt.
  </step>
  <step order="2">
  Delegate to the `scrumai.architect` subagent with following prompt:
  ```
  Fill the design with a technical solution and implementation plan for `${DIRECTORY}/spec.md`.
  ```
  </step>
</steps>

<postValidate>
- Validate the design is complete and feasible; loop with the architect until it is.
- Tick the **② Design** items in `${DIRECTORY}/checklist.md`.
</postValidate>

<output>
- `${DIRECTORY}/design.md`
- `${DIRECTORY}/checklist.md` (③ tasks added, ② items ticked)
- Report readiness for `/scrumai.start` or `/scrumai.start-full`.
</output>
