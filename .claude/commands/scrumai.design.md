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
  Delegate to the `scrumai.architect` subagent with EXACT following prompt:
  ```
  Let analyze thoroughly the `${DIRECTORY}/spec.md` and codebase to produce a design for it.
  1. Use the `.claude/templates/design.md` template to create `${DIRECTORY}/design.md`.
  2. Check the ② Design items in `${DIRECTORY}/checklist.md` as you satisfy them.
  ```
  </step>
  <step order="3">
  Delegate to the `scrumai.architect` subagent with EXACT following prompt:
  ```
  Let analyze thoroughly the `${DIRECTORY}/design.md` and `${DIRECTORY}/spec.md` to list the test cases:
  1. Use the `.claude/template/test.md` template to create `${DIRECTORY}/test.md`.
  2. Check the ② Design items in `${DIRECTORY}/checklist.md` as you satisfy them.
  ```
  </step>
</steps>

<postValidate>
- Validate the design is complete and feasible; loop with the architect until it is.
- Validate the `design.md` contains the Implementation Plan table and the Mermaid dependency graph.
- Validate the ③ Implement declare list of task checklist in the `${DIRECTORY}/checklist.md`.
- Tick the **② Design** items in `${DIRECTORY}/checklist.md`.
</postValidate>

<output>
- `${DIRECTORY}/design.md` (incl. Implementation Plan table + Mermaid dependency/parallelization graph)
- `${DIRECTORY}/checklist.md` (③ tasks added with waves + dependencies, ② items ticked)
- `${DIRECTORY}/test.md` (test cases per requirement)
- Report readiness for `/scrumai.start` or `/scrumai.start-full`
</output>
