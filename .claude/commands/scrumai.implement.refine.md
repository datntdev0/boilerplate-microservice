---
description: Take human feedback on a verified feature and update the implementation and related documents to match.
argument-hint: <feedback text> [feature folder] (folder defaults to latest)
# allowed-tools: AskUserQuestion, Read, Write, Edit, Glob, Grep, Bash, Agent
---

# /scrumai.implement.refine — Phase ⑤ Refine

> Skeleton. Author the prompt body below. Goal: act on the human's feedback after verification —
> adjust code and keep `spec.md` / `design.md` / `plan.md` / `test.md` in sync. Iterative; re-run
> after each round of feedback.

## Inputs
- `$ARGUMENTS`: the human's feedback (free text).
- Active feature folder (arg, or most recent `.scrumai/features/<NNN>-<name>/`).
- `spec.md`, `design.md`, `plan.md`, the verification result `test.md`, and the local diff.
- Shared conventions: load the `scrumai-conventions` skill.
- Project principles: `.claude/memory/constitution.md`.

## Steps (TODO: flesh out)
1. Read the feature docs + `test.md`; ensure on the feature branch (not `main`).
2. **Clarify the feedback** if ambiguous — use `AskUserQuestion` (or the `scrumai.clarifier`
   subagent) so each item is concrete and actionable. Record the feedback in `test.md` (Follow-ups).
3. **Triage** each feedback item and route it to the right altitude:
   - requirement change → update `spec.md` (re-clarify if scope shifts),
   - approach / task change → delegate to `scrumai.architect`; update `design.md` (incl. Test Cases) and `plan.md`,
   - implementation defect / tweak → fix the code directly.
4. **Apply the changes** following repo idioms; build (`dotnet build` / `ng build`).
5. **Re-check** the affected parts:
   - internal review via `scrumai.reviewer`,
   - re-test via `scrumai.tester` (unit + `tools-playwright` manual where UI changed); refresh `evidence/`.
6. **Update `test.md`** with new results and mark each feedback item resolved.
7. **Commit** the refinement with a conventional message.
8. Do NOT push. If feedback remains or new feedback arrives, repeat.

## Output
- Updated code + synced documents (`spec.md` / `design.md` / `plan.md` / `test.md`).
- Refreshed `evidence/` for re-tested scenarios.
- Local commit(s) for this refinement round.
- Report what changed and whether the feature is ready (or what feedback is still open).
