---
description: Perform local code review and manual/unit/web testing against the documents, capturing evidence.
argument-hint: feature folder, e.g. .scrumai/features/001-user-auth (defaults to latest)
allowed-tools: Read, Write, Glob, Grep, Bash, Agent
---

# /scrumai.verify — Phase ④ Verify

<goal>
Perform unit tests and manual testing against the spec/design.
</goal>

<inputs>
- `$DIRECTORY`: resolve the feature directory name provided by the human. E.g: `.scrumai/features/<name>/`.
- Shared conventions: load the `scrumai-conventions` skill.
</inputs>

<steps>
  <step order="1">
  - Resolve the `$DIRECTORY` and check the `${DIRECTORY}/checklist.md` exists.
  - If any are missing, report and halt.
  </step>
  <step order="1">
  Delegate to the `scrumai.tester` subagent with EXACT following prompt:
  ```
  Perform unit tests for the feature in `${DIRECTORY}` according **Unit Test** sections in `${DIRECTORY}/test.md`.
  ``` 
  </step>
  <step order="2">
  Delegate to the `scrumai.tester` subagent with EXACT following prompt:
  ```
  Perform manual web/UI testing for the feature in `${DIRECTORY}` according **Manual tests (web/UI)** sections in `${DIRECTORY}/test.md`.
  ```
  </step>
  <step order="3">
  Update the `${DIRECTORY}/test.md` with the test results, evidence links, and a PASS/FAIL verdict. Fill the **Environment** and **Test Data** sections (accounts, inputs, created IDs, config, log/artifact paths) so the run can be debugged or reproduced later.
  </step>
  <step order="4">
  Review the test results and determine a final PASS/FAIL verdict for the feature. If any acceptance scenario fails, resolve the blockers (e.g. fix code, update spec/design, or add more tests) and repeat verification until a clean PASS is achieved
   </step>
  </step>
</steps>

<postValidate>
- Validate all **Unit Test** item passed and documented in `${DIRECTORY}/test.md`. Loop until clean.
- Validate all **Manual tests (web/UI)** item passed and documented in `${DIRECTORY}/test.md`. Loop until clean.
- Tick the **④ Verify** items in `${DIRECTORY}/checklist.md`.
</postValidate>

<output>
- `${DIRECTORY}/test.md`
- `${DIRECTORY}/evidence/`
- `${DIRECTORY}/checklist.md` (④ items ticked)
- Report verdict or suggest the `scrumai.refine` command.
</output>
