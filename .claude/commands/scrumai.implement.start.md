---
description: Execute the implementation plan, complete coding tasks, and commit to the local repository.
argument-hint: [feature folder] (defaults to latest)
# allowed-tools: Read, Write, Edit, Glob, Grep, Bash, Agent
---

# /scrumai.implement.start — Phase ③ Implement

> Skeleton. Author the prompt body below. Goal: work `plan.md` to completion,
> committing locally per task with conventional messages.

## Inputs
- Active feature folder (arg, or most recent `.scrumai/features/<NNN>-<name>/`).
- `design.md` + `plan.md` from Phase ②.
- Shared conventions: load the `scrumai-conventions` skill (commit format, build/test commands).

## Steps (TODO: flesh out)
1. Read design + plan; ensure on a feature branch (not `main`).
2. For each task in order:
   - implement the change following repo idioms and the modular DI system,
   - build (`dotnet build` / `ng build`) to confirm it compiles,
   - commit locally with a conventional message (feat/fix/refactor/build/chore),
   - check the task off in `plan.md`.
3. Internal code review — once all tasks are done, delegate to the `scrumai.reviewer`
   subagent (enforces `.claude/memory/constitution.md`) to review the changes against
   `spec.md` / `design.md`. Fix the findings and commit the fixes. This is a self-review
   pass before handing off to Phase ④.
4. Do NOT push; leave verification to Phase ④.

## Output
- Code changes + local commits.
- Updated `plan.md` checklist.
- Internal review findings addressed (`scrumai.reviewer`).
- Report readiness for `/scrumai.implement.verify`.
