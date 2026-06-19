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
  Implement the ③ tasks **wave by wave**, using the dependency graph in `${DIRECTORY}/design.md`. Process waves in ascending order; do not start a wave until the previous one is fully committed. For each completed task, check it off in `${DIRECTORY}/checklist.md`.
  </step>
  <step order="3">
  Delegate to the `scrumai.reviewer` subagent with EXACT following prompt:
  ```
  Perform an internal code review against the `${DIRECTORY}/spec.md` and `${DIRECTORY}/design.md`. 
  Fill the review findings into the **Code Review Finding** section in `${DIRECTORY}/test.md`.
  ```
  </step>
  <step order="4">
  1. Loop until there is no more unchecked **Code Review Finding** in `${DIRECTORY}/test.md`.
  2. For each finding, fix the code and ensure the application build succesfuly.
  3. Check the working review finding off in `${DIRECTORY}/test.md`.
  </step>
</steps>

<postValidate>
- Validate the application builds successfully (`dotnet build` / `ng build`). Loop until clean.
- Validate all tasks are checked off in `${DIRECTORY}/checklist.md`. Loop until clean.
- Validate all review findings are fixed in `${DIRECTORY}/test.md`. Loop until clean.
- Tick the **③ Implement** items in `${DIRECTORY}/checklist.md`.
- Do NOT push; leave verification to Phase ④.
</postValidate>

<output>
- Code changes + application build success.
- Internal review findings addressed (`scrumai.reviewer`).
- `${DIRECTORY}/checklist.md` with ③ tasks and gate items checked.
- Report readiness for `/scrumai.verify`.
</output>
