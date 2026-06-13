---
description: Interactively clarify a requirement with the human and produce a specification document.
argument-hint: <free-text requirement, or feature folder to refine>
# allowed-tools: AskUserQuestion, Read, Write, Glob, Grep, Agent
---

# /scrumai.requirement.specify — Phase ① Specify

> Skeleton. Author the prompt body below. Goal: turn a raw requirement into a
> reviewed `spec.md` after a structured clarification loop with the human.

## Inputs
- `$ARGUMENTS`: the requirement description (or an existing `.scrumai/features/NNN-<name>/` to refine).
- Shared conventions: load the `scrumai-conventions` skill.

## Steps (TODO: flesh out)
1. Derive a 2–4 word kebab-case feature `<name>` from the requirement.
2. Resolve the feature ID `NNN` — a zero-padded 3-digit sequential number:
   - Scan existing `.scrumai/features/` for folders matching `NNN-*`.
   - Take the highest `NNN`, add 1; if none exist, start at `001`.
   - The folder name is `<NNN>-<name>` (e.g. `001-form-tagify`).
3. Resolve the feature folder: `.scrumai/features/<NNN>-<name>/`.
4. Delegate elicitation to the `scrumai.clarifier` subagent — ask only high-impact
   questions, max ~3–5, using option-based prompts. Make informed defaults otherwise.
5. Copy `.claude/templates/spec.md` → `.scrumai/features/<NNN>-<name>/spec.md`
   and fill it with concrete, testable, technology-agnostic requirements.
6. Validate the spec is complete and unambiguous; loop until clean.

## Output
- `.scrumai/features/<NNN>-<name>/spec.md`
- Report the folder path and readiness for `/scrumai.implement.design`.
