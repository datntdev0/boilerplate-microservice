---
description: Interactively clarify a requirement with the human and produce a specification document.
argument-hint: <free-text requirement, or feature folder to refine>
allowed-tools: AskUserQuestion, Read, Write, Glob, Grep, Agent
---

# /scrumai.requirement.specify — Phase ① Specify

<goal>
Turn a raw requirement into a reviewed `spec.md` after a structured clarification loop with the human.
</goal>

<inputs>
- `$ARGUMENTS`: the requirement description (or an existing `.scrumai/features/NNN-<name>/` to refine).
- Shared conventions: load the `scrumai-conventions` skill.
</inputs>

<steps>
   <step order="1">
   Derive a 2–4 word kebab-case feature `<name>` from the requirement.
   </step>
   <step order="2">
   Resolve the feature ID `NNN` — a zero-padded 3-digit sequential number:
   - Scan existing `.scrumai/features/` for folders matching `NNN-*`.
   - Take the highest `NNN`, add 1; if none exist, start at `001`.
   - The folder name is `<NNN>-<name>` (e.g. `001-form-tagify`).
   </step>
   <step order="3">
   Delegate elicitation to the `scrumai.clarifier` subagent:
   - Ask only high-impact questions, max ~3–5, using option-based prompts.
   - Make informed defaults otherwise.
   </step>
   <step order="4">
   Fill the spec with concrete, testable, technology-agnostic requirements.
   - Copy `.claude/templates/spec.md` → `.scrumai/features/<NNN>-<name>/spec.md`.
   - Copy `.claude/templates/checklist.md` → `.scrumai/features/<NNN>-<name>/checklist.md`.
   </step>
</steps>

<postValidate>
- Validate the spec is complete and unambiguous; loop until clean.
- Tick the **① Specify** items in `checklist.md`. Honor checklist gating (see `scrumai-conventions`).
</postValidate>

<output>
- `.scrumai/features/<NNN>-<name>/spec.md`
- `.scrumai/features/<NNN>-<name>/checklist.md` (① items ticked)
- Report the folder path and readiness for `/scrumai.implement.design` or `/scrumai.requirement.full`.
</output>