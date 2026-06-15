# Checklist: [FEATURE NAME]

**Feature**: `<name>`

> **Gate rule.** A phase is complete only when **every required item below is checked**. An agent
> MUST NOT advance to the next phase, hand off, or report "done" while any required item is
> unchecked. If an item cannot be met, **STOP and report what is blocking** — never skip a gate
> silently. Tick each item as it is satisfied.

## ① Specify
- [ ] `spec.md` written with acceptance scenarios
- [ ] No open `[NEEDS CLARIFICATION]` markers
- [ ] Success criteria are measurable

## ② Design
- [ ] `design.md` approach + affected components recorded
- [ ] Unit test cases defined (each tied to a requirement)
- [ ] Manual test cases defined **or** `design.md` states "N/A — no UI/web behavior"
- [ ] Implementation tasks listed under ③ below (ordered, independently parallelizable)

## ③ Implement
> Tasks — filled in by `/scrumai.design`, checked off by `/scrumai.start`:
- [ ] T1 — [task] · _commit:_ `type: …`
- [ ] T2 — [task] · _commit:_ `type: …`

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
