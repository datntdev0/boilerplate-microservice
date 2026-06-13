---
description: Execute the implementation plan, complete coding tasks, and commit to the local repository.
argument-hint: feature folder, e.g. .scrumai/features/001-user-auth (defaults to latest)
allowed-tools: Read, Write, Edit, Glob, Grep, Bash, Agent
---

# /scrumai.implement.start — Phase ③ Implement

<goal>
Work `plan.md` to completion on a feature branch, committing locally with conventional messages.
</goal>

<inputs>
- Active feature folder (arg, or most recent `.scrumai/features/<NNN>-<name>/`).
- `design.md` + `plan.md` from Phase ②.
- Shared conventions: load the `scrumai-conventions` skill (commit format, build/test commands).
</inputs>

<steps>
   <step order="1">
   Read design + plan. Create (or switch to) the git branch `feat/<feature_directory_name>`
   — the feature folder name, e.g. `feat/001-form-tagify` — branched off `main`. Never work on `main`.
   </step>
   <step order="2">
   For each task in order:
   - implement the change following repo idioms and the modular DI system,
   - build (`dotnet build` / `ng build`) to confirm it compiles,
   - commit locally with a conventional message (feat/fix/refactor/build/chore),
   - check the task off in `plan.md`.
   </step>
   <step order="3">
   Internal code review — once all tasks are done, delegate to the `scrumai.reviewer` subagent
   (enforces `.claude/memory/constitution.md`) to review the changes against `spec.md` / `design.md`.
   Fix the findings and commit the fixes. This is a self-review pass before handing off to Phase ④.
   </step>
</steps>

<postValidate>
- Tick the **③ Implement** items in `checklist.md`. Honor checklist gating (see `scrumai-conventions`):
  do not report readiness while any ③ item is unchecked; if blocked, stop and report.
- Do NOT push; leave verification to Phase ④.
</postValidate>

<output>
- Code changes + local commits.
- Updated `plan.md` checklist.
- Internal review findings addressed (`scrumai.reviewer`).
- `checklist.md` with ③ items ticked.
- Report readiness for `/scrumai.implement.verify`.
</output>
