---
description: Take human feedback on a verified feature and update the implementation and related documents to match.
argument-hint: <feedback text> [feature folder] (folder defaults to latest)
allowed-tools: AskUserQuestion, Read, Write, Edit, Glob, Grep, Bash, Agent
---

# /scrumai.refine — Phase ⑤ Refine

<goal>
Act on the human's feedback after verification — adjust code and keep `spec.md` / `design.md` / `test.md` and the tasks in `checklist.md` in sync. Iterative; re-run after each round of feedback.
</goal>

<inputs>
- `$ARGUMENTS`: the human's feedback (free text).
- `$DIRECTORY`: resolve the feature directory name provided by the human. E.g: `.scrumai/features/<name>/`.
- Project principles: `.claude/memory/constitution.md`.
</inputs>

<steps>
  <step order="1">
  - Resolve the `$DIRECTORY` and check the `${DIRECTORY}/checklist.md` exists.
  - If any are missing, report and halt.
  </step>
  <step order="2">
  **Clarify the feedback** if ambiguous — use `AskUserQuestion` (or the `scrumai.clarifier` subagent) so each item is concrete and actionable. Record the feedback in `test.md` (Follow-ups).
  </step>
  <step order="3">
  **Triage** each feedback item and route it to the right altitude:
  - requirement change → update `spec.md` (re-clarify if scope shifts),
  - approach or task change → delegate to `scrumai.architect` to update `design.md`, `test.md`, and `checklist.md` (re-clarify if scope shifts),
  - implementation defect / tweak → fix the code directly.
  </step>
  <step order="4">
  **Apply the changes** following repo idioms;
  </step>
  <step order="5">
  **Re-check** the affected parts:
  - internal review via `scrumai.reviewer`,
  - re-test via `scrumai.tester`.
  </step>
  <step order="6">
  **Commit** the refinement with a conventional message.
  </step>
</steps>

<postValidate>
- Re-tick the affected `checklist.md` items:
  - Validate the spec is complete and unambiguous; loop until clean.
  - Validate the design is complete and feasible; loop with the architect until it is.
  - Validate the `design.md` contains the Implementation Plan table and the Mermaid dependency graph.
  - Validate the application builds successfully (`dotnet build` / `ng build`). Loop until clean.
  - Validate all tasks are checked off in `${DIRECTORY}/checklist.md`. Loop until clean.
  - Validate all review findings are fixed in `${DIRECTORY}/test.md`. Loop until clean.
- Do NOT push. If feedback remains or new feedback arrives, repeat.
</postValidate>

<output>
- Code changes + application build success.
- Updated affected artifacts in `${DIRECTORY}`.
- Report what changed and whether the feature is ready (or what feedback is still open).
</output>
