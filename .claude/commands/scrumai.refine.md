---
description: Take human feedback on a verified feature and update the implementation and related documents to match.
argument-hint: <feedback text> [feature folder] (folder defaults to latest)
allowed-tools: AskUserQuestion, Read, Write, Edit, Glob, Grep, Bash, Agent
---

# /scrumai.refine — Phase ⑤ Refine

<goal>
Act on the human's feedback after verification — adjust code and keep `spec.md` / `design.md` /
`test.md` and the tasks in `checklist.md` in sync. Iterative; re-run after each round of feedback.
</goal>

<inputs>
- `$ARGUMENTS`: the human's feedback (free text).
- Active feature folder (arg, or most recent `.scrumai/features/<name>/`).
- `spec.md`, `design.md`, `checklist.md`, the verification result `test.md`, and the local diff.
- Shared conventions: load the `scrumai-conventions` skill.
- Project principles: `.claude/memory/constitution.md`.
</inputs>

<steps>
   <step order="1">
   Read the feature docs + `test.md`; ensure on the feature branch (not `main`).
   </step>
   <step order="2">
   **Clarify the feedback** if ambiguous — use `AskUserQuestion` (or the `scrumai.clarifier` subagent)
   so each item is concrete and actionable. Record the feedback in `test.md` (Follow-ups).
   </step>
   <step order="3">
   **Triage** each feedback item and route it to the right altitude:
   - requirement change → update `spec.md` (re-clarify if scope shifts),
   - approach / task change → delegate to `scrumai.architect`; update `design.md` (incl. Test Cases) and the ③ tasks in `checklist.md`,
   - implementation defect / tweak → fix the code directly.
   </step>
   <step order="4">
   **Apply the changes** following repo idioms; build (`dotnet build` / `ng build`).
   </step>
   <step order="5">
   **Re-check** the affected parts:
   - internal review via `scrumai.reviewer`,
   - re-test via `scrumai.tester` (unit + `tools-playwright` manual where UI changed); refresh `evidence/`.
   </step>
   <step order="6">
   **Update `test.md`** with new results and mark each feedback item resolved.
   </step>
   <step order="7">
   **Commit** the refinement with a conventional message.
   </step>
</steps>

<postValidate>
- Re-tick the affected `checklist.md` items (④ in particular: re-run manual tests where UI changed)
  and tick the **⑤ Refine** items. Honor checklist gating (see `scrumai-conventions`): do not report
  the feature ready while any required item is unchecked.
- Do NOT push. If feedback remains or new feedback arrives, repeat.
</postValidate>

<output>
- Updated code + synced documents (`spec.md` / `design.md` / `test.md` + tasks in `checklist.md`).
- Refreshed `evidence/` for re-tested scenarios.
- `checklist.md` re-ticked (④ re-verified, ⑤ ticked).
- Local commit(s) for this refinement round.
- Report what changed and whether the feature is ready (or what feedback is still open).
</output>
