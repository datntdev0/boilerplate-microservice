---
description: Run the full implementation workflow (design → implement → verify) autonomously from a ready spec, then stop for human review.
argument-hint: [feature folder, e.g. .scrumai/features/001-user-auth] (defaults to latest)
# allowed-tools: Workflow
---

# /scrumai.implement.full — Autonomous ②③④

Launch the saved workflow that runs **Design → Implement → Verify** without interruption and stops
after Verify so the human can review the final result.

## Prerequisite
- A ready `spec.md` exists in the target feature folder (run `/scrumai.requirement.specify` first).

## Action
Call the **Workflow** tool with:
- `scriptPath: ".claude/workflows/scrumai.implement.full.ts"`
- `args: { "feature": "<feature folder from $ARGUMENTS, or empty string to use the latest>" }`

This is an explicit opt-in to multi-agent orchestration: the workflow spawns a subagent per phase
(Design, Implement, Verify) and may run many tool calls / commits. Do not run other work meanwhile.

## On completion
- Summarize the `test.md` verdict (PASS/FAIL), the commits made, and the changed documents.
- Do **not** push.
- If the human has feedback, point them to `/scrumai.implement.refine`.
