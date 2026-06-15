---
name: scrumai-conventions
description: Shared conventions for the ScrumAI workflow — repo layout, build/test commands, commit format, and where feature documents live. Loaded by the scrumai.* commands and agents.
user-invocable: false
---

# ScrumAI Conventions (skeleton)

> Author the shared knowledge below. Keep it the single source of truth so the
> four commands and their agents stay consistent.

## Feature artifacts
- One folder per feature: `.scrumai/features/<name>/` — `<name>` is the feature directory name
  (kebab-case) the human provides when invoking a command. No auto-numbering.
- Files: `spec.md`, `design.md`, `test.md`, `checklist.md`, `evidence/`.
- Implementation tasks live in `checklist.md` under ③ Implement — there is no separate `plan.md`.

## Checklist gating
- Every feature folder has a `checklist.md` (from `.claude/templates/checklist.md`) with per-phase gates.
- A phase is **complete only when all its required items are checked**. An agent MUST NOT advance to
  the next phase, hand off, or report "done" while any required item is unchecked.
- If a required item cannot be met, **STOP and report what is blocking** — never skip a gate
  silently (e.g. do not skip manual tests). Each phase ticks its own items as it satisfies them.

## Git branch
- Implementation runs on `feat/<name>` — the feature directory name
  (e.g. `feat/form-tagify`), branched off `main`. Created in `/scrumai.implement.start`. Never commit on `main`.

## Commit format (conventional)
- Types seen in this repo: `feat` `fix`/`bugs` `refactor` `build` `chore`.

## Build / test commands
- Backend build: `dotnet build datntdev.Microservice.slnf`
- Backend tests: `dotnet test --settings .runsettings`
- Angular (from srcs/apps/Angular): `npm start`, `ng build`, `npm run test:ci`
- E2E (from e2e/): `npx playwright test`
- Migrations: run the Migrator project.

## Architecture reminders
- DDD/SOLID; modular `BaseModule` `[DependOn]` DI; YARP gateway; OpenIddict auth.

