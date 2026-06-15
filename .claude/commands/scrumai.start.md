---
description: Execute the implementation tasks, complete coding work, and commit to the local repository.
argument-hint: feature folder, e.g. .scrumai/features/user-auth (defaults to latest)
allowed-tools: Read, Write, Edit, Glob, Grep, Bash, Agent
---

# /scrumai.start — Phase ③ Implement

<goal>
Work the ③ Implement tasks in `checklist.md` to completion on a feature branch, committing locally with conventional messages.
</goal>

<inputs>
- `$DIRECTORY`: resolve the feature directory name provided by the human. E.g: `.scrumai/features/<name>/`.
- Shared conventions: load the `scrumai-conventions` skill (commit format, build/test commands).
</inputs>

<steps>
  <step order="1">
  - Resolve the `$DIRECTORY` and check the `${DIRECTORY}/checklist.md` exists.
  - If any are missing, report and halt.
  </step>
  <step order="2">
  For each ③ task in the `${DIRECTORY}/checklist.md`, perform the following steps until all are done:
  1. implement the change following repo idioms and the modular DI system,
  2. build (`dotnet build` / `ng build`) to confirm it compiles,
  3. commit locally with a conventional message (feat/fix/refactor/build/chore),
  4. check the task off in `${DIRECTORY}/checklist.md` (③).
  </step>
  <step order="3">
  Delegate to the `scrumai.reviewer` subagent with following prompt:
  ```
  Perform an internal code review against the `${DIRECTORY}/spec.md` and `${DIRECTORY}/design.md`. 
  Copy the `.claude/templates/review.md` → `${DIRECTORY}/test.md` if not exists and fill the review findings.
  ```
  </step>
  <step order="4">
  For each review finding in `${DIRECTORY}/test.md`, address by fixing code:
  1. implement the code change for the review finding
  2. build to confirm it compiles successfully
  3. commit locally with a conventional message (feat/fix/refactor/build/chore)
  4. update status for the finding in `${DIRECTORY}/test.md` to "fixed" and add a reference to the commit hash that fixed it.
  </step>
</steps>

<postValidate>
- Validate all tasks are checked off in `${DIRECTORY}/checklist.md`. Loop until clean.
- Validate all review findings are fixed in `${DIRECTORY}/test.md`. Loop until clean.
- Tick the **③ Implement** items in `${DIRECTORY}/checklist.md`.
- Do NOT push; leave verification to Phase ④.
</postValidate>

<output>
- Code changes + local commits.
- `${DIRECTORY}/checklist.md` with ③ tasks and gate items checked.
- Internal review findings addressed (`scrumai.reviewer`).
- Report readiness for `/scrumai.verify`.
</output>
