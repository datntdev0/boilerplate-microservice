---
description: Interactively clarify a requirement with the human and produce a specification document.
argument-hint: <feature-dir-name> <free-text requirement> (or an existing .scrumai/features/<name>/ to refine)
allowed-tools: AskUserQuestion, Read, Write, Glob, Grep, Agent
---

# /scrumai.requirement.specify — Phase ① Specify

<goal>
Turn a raw requirement into a reviewed `spec.md` after a structured clarification loop with the human.
</goal>

<inputs>
- `$ARGUMENTS`: the feature directory name (kebab-case) and the requirement description — or an
  existing `.scrumai/features/<name>/` to refine.
- Shared conventions: load the `scrumai-conventions` skill.
</inputs>

<steps>
   <step order="1">
   Determine `<name>` — the feature directory name. The human provides it when invoking the command;
   do not auto-number. If only a requirement was given without a name, propose a 2–4 word kebab-case
   name and confirm with the human. The folder is `.scrumai/features/<name>/`.
   </step>
   <step order="2">
   Delegate elicitation to the `scrumai.clarifier` subagent:
   - Ask only high-impact questions, max ~3–5, using option-based prompts.
   - Make informed defaults otherwise.
   </step>
   <step order="3">
   Fill the spec with concrete, testable, technology-agnostic requirements.
   - Copy `.claude/templates/spec.md` → `.scrumai/features/<name>/spec.md`.
   - Copy `.claude/templates/checklist.md` → `.scrumai/features/<name>/checklist.md`.
   </step>
</steps>

<postValidate>
- Validate the spec is complete and unambiguous; loop until clean.
- Tick the **① Specify** items in `checklist.md`. Honor checklist gating (see `scrumai-conventions`).
</postValidate>

<output>
- `.scrumai/features/<name>/spec.md`
- `.scrumai/features/<name>/checklist.md` (① items ticked)
- Report the folder path and readiness for `/scrumai.implement.design` or `/scrumai.implement.full`.
</output>
