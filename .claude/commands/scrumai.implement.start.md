---
description: Execute the implementation tasks, complete coding work, and commit to the local repository.
argument-hint: feature folder, e.g. .scrumai/features/user-auth (defaults to latest)
allowed-tools: Read, Write, Edit, Glob, Grep, Bash, Agent
---

# /scrumai.implement.start — Phase ③ Implement

<goal>
Work the ③ Implement tasks in `checklist.md` to completion on a feature branch, committing locally
with conventional messages.
</goal>

<inputs>
- Active feature folder (arg, or most recent `.scrumai/features/<name>/`).
- `design.md` + the ③ Implement tasks in `checklist.md` from Phase ②.
- Shared conventions: load the `scrumai-conventions` skill (commit format, build/test commands).
</inputs>

<steps>
   <step order="1">
   Read design + the ③ tasks. Create (or switch to) the git branch `feat/<name>`
   — the feature folder name, e.g. `feat/form-tagify` — branched off `main`. Never work on `main`.
   </step>
   <step order="2">
   For each ③ task in order:
   - implement the change following repo idioms and the modular DI system,
   - build (`dotnet build` / `ng build`) to confirm it compiles,
   - commit locally with a conventional message (feat/fix/refactor/build/chore),
   - check the task off in `checklist.md` (③).
   </step>
   <step order="3">
   Internal code review — once all tasks are done, delegate to the `scrumai.reviewer` subagent
   (enforces `.claude/memory/constitution.md`) to review the changes against `spec.md` / `design.md`.
   Fix the findings and commit the fixes. This is a self-review pass before handing off to Phase ④.
   </step>
</steps>

<postValidate>
- Tick the **③ Implement** gate items in `checklist.md`. Honor checklist gating (see `scrumai-conventions`):
  do not report readiness while any ③ item is unchecked; if blocked, stop and report.
- Do NOT push; leave verification to Phase ④.
</postValidate>

<output>
- Code changes + local commits.
- `checklist.md` with ③ tasks and gate items checked.
- Internal review findings addressed (`scrumai.reviewer`).
- Report readiness for `/scrumai.implement.verify`.
</output>
