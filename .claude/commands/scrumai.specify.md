---
description: Interactively clarify a requirement with the human and produce a specification document.
argument-hint: <feature-dir-name> <free-text requirement> (or an existing .scrumai/features/<name>/ to refine)
allowed-tools: AskUserQuestion, Read, Write, Glob, Grep, Agent
---

# /scrumai.specify — Phase ① Specify

<goal>
Turn a raw requirement into a reviewed `spec.md` after a structured clarification loop with the human.
</goal>

<inputs>
- `$DIRECTORY`: resolve the feature directory name provided by the human. E.g: `.scrumai/features/<name>/`.
- `$REQUIREMENT`: the initial requirement text provided by the human. Can be vague and incomplete.
- Shared conventions: load the `scrumai-conventions` skill.
</inputs>

<steps>
  <step order="1">
  - Resolve the `$DIRECTORY`, the human provides it when invoking the command;
  - If only a requirement was given without a name, propose a 2–4 word kebab-case name.
  - If the directory doesn't exist, create it. Otherwise use the existing directory.
  </step>
  <step order="2">
  Delegate elicitation to the `scrumai.clarifier` subagent with following prompt:
  ```
  Fill the spec with concrete, testable, technology-agnostic requirements for `$REQUIREMENT`.
  - Copy `.claude/templates/spec.md` → `${DIRECTORY}/spec.md`.
  - Copy `.claude/templates/checklist.md` → `${DIRECTORY}/checklist.md`.
  ```
  </step>
</steps>

<postValidate>
- Validate the spec is complete and unambiguous; loop until clean.
- Tick the **① Specify** items in `${DIRECTORY}/checklist.md`.
</postValidate>

<output>
- `${DIRECTORY}/spec.md`
- `${DIRECTORY}/checklist.md` (① items ticked)
- Report the folder path and readiness for `/scrumai.design` or `/scrumai.start-full`.
</output>