# Checklist: [FEATURE NAME]

**Feature**: `<name>`

> **Gate rule.** A phase is complete only when **every required item below is checked**.
> An agent MUST NOT advance to the next phase, hand off, or report "done" while any required item is unchecked.
> If an item cannot be met, **STOP and report what is blocking** — never skip a gate silently. Tick each item as it is satisfied.

## ① Specify
- [ ] `spec.md` written with acceptance scenarios
- [ ] No open `[NEEDS CLARIFICATION]` markers

## ② Design
- [ ] `design.md` approach + affected components recorded
- [ ] `design.md` has the Implementation Plan table + Mermaid dependency graph
- [ ] Implementation tasks listed under ③ below, grouped into parallel waves with declared dependencies
- [ ] `test.md` Unit test cases defined (each tied to a requirement)
- [ ] `test.md` Manual test cases defined **or** `design.md` states "N/A — no UI/web behavior"

## ③ Implement
> Tasks — filled in by `/scrumai.design` (with waves + dependencies), checked off by `/scrumai.start`.
> Tasks in the same **wave** are independent and may be implemented in parallel by separate agents;
> the dependency graph lives in `design.md`.
- [ ] T1 _(wave 1 · deps: —)_ — [task] · _commit:_ `type: …`
- [ ] T2 _(wave 1 · deps: —)_ — [task] · _commit:_ `type: …`
- [ ] T3 _(wave 2 · deps: T1, T2)_ — [task] · _commit:_ `type: …`

Gates:
- [ ] On branch `feat/<name>` (not `main`)
- [ ] All tasks above done and checked off
- [ ] Build passes (`dotnet build` / `ng build`)
- [ ] Internal review (`scrumai.reviewer`) clean; findings fixed

## ④ Verify
- [ ] Code review clean; findings fixed
- [ ] Unit tests executed and passing (evidence saved)
- [ ] Manual tests executed and passing (evidence saved)
- [ ] `test.md` verdict recorded (PASS/FAIL)

## ⑤ Refine (per feedback round)
- [ ] Every feedback item triaged and addressed
- [ ] Documents synced (`spec.md` / `design.md` / `test.md` + tasks in `checklist.md`)
- [ ] Re-review and re-test passed (manual tests re-run where UI changed)
