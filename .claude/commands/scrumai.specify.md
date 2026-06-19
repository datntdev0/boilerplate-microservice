---
description: Interactively clarify a requirement with the human and produce a specification document.
argument-hint: <feature-dir-name> <free-text requirement>
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
  1. Resolve the `$DIRECTORY`, the human provides it when invoking the command;
  2. If only a requirement was given without a name, propose a 2–4 word kebab-case name.
  3. If the directory doesn't exist, create it. Otherwise use the existing directory.
  </step>
  <step order="2">
  Delegate elicitation to the `scrumai.clarifier` subagent with EXACT following prompt:
  ```
  Let analyze the `$REQUIREMENT`.
  1. Use the `.claude/templates/spec.md` template to create `${DIRECTORY}/spec.md`.
  2. Use the `.claude/templates/checklist.md` template to create `${DIRECTORY}/checklist.md`.
  3. Check the ① Specify items in `${DIRECTORY}/checklist.md` as you satisfy them.
  ```
  </step>
  <step order="3">
  1. Loop until there is no more `[NEEDS CLARIFICATION]` marker in `${DIRECTORY}/spec.md`.
  2. For each marker, ask the human a question to clarify it, then update the `spec.md` accordingly. 
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