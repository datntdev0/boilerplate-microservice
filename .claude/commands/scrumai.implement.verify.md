---
description: Perform local code review and manual/unit/web testing against the documents, capturing evidence.
argument-hint: feature folder, e.g. .scrumai/features/001-user-auth (defaults to latest)
allowed-tools: Read, Write, Glob, Grep, Bash, Agent
---

# /scrumai.implement.verify — Phase ④ Verify

<goal>
Review the implementation and test it against the spec/design, recording findings + evidence in `test.md`.
</goal>

<inputs>
- Active feature folder (arg, or most recent `.scrumai/features/<NNN>-<name>/`).
- `spec.md`, `design.md`, `plan.md`, and the local diff.
- Shared conventions: load the `scrumai-conventions` skill.
</inputs>

<steps>
   <step order="1">
   Code review — delegate to `scrumai.reviewer` (enforces `.claude/memory/constitution.md`):
   correctness vs spec, DDD/SOLID, modular DI, contract/migration safety.
   </step>
   <step order="2">
   Testing — delegate to `scrumai.tester`:
   - unit: `dotnet test --settings .runsettings`, `npm run test:ci`,
   - manual web/UI: **run every manual test case listed in `design.md`** (skip ONLY if `design.md`
     recorded "N/A — no UI/web behavior"). Start the app, then interactively drive a browser via the
     `tools-playwright` skill to walk each case (dev-time manual testing, not the `e2e/` suite).
     Hard gate — do not mark verification done with manual cases unexecuted.
   - capture logs, coverage, and screenshots into `evidence/`.
   </step>
   <step order="3">
   Write `test.md` from `.claude/templates/test.md`: findings, test results, evidence links, and a PASS/FAIL verdict.
   </step>
</steps>

<postValidate>
- Tick the **④ Verify** items in `checklist.md`. Honor checklist gating (see `scrumai-conventions`):
  the verdict is **not** PASS while any ④ item is unchecked — especially "Manual tests executed".
  If a gate cannot be met, stop and report it rather than passing.
</postValidate>

<output>
- `.scrumai/features/<NNN>-<name>/test.md`
- `.scrumai/features/<NNN>-<name>/evidence/`
- `checklist.md` with ④ items ticked (or the blocking gate reported)
- Report verdict and any follow-up tasks.
</output>
