---
description: Run the full implementation workflow (design → implement → verify) autonomously from a ready spec, then stop for human review.
argument-hint: feature folder, e.g. .scrumai/features/user-auth (defaults to latest)
allowed-tools: Workflow
---

# /scrumai.start-full — Autonomous ②③④

<goal>
Run the saved workflow Design → Implement → Verify without interruption from a ready spec, then stop
after Verify so the human can review the final result.
</goal>

<prerequisite>
- A ready `spec.md` exists in the target feature folder (run `/scrumai.specify` first).
</prerequisite>

<action>
Call the **Workflow** tool with:
- `scriptPath: ".claude/workflows/scrumai.implement.full.ts"`
- `args: { "feature": "<feature folder from $ARGUMENTS, or empty string to use the latest>" }`

Explicit opt-in to multi-agent orchestration: one subagent per phase (Design, Implement, Verify),
many tool calls / commits. Do not run other work meanwhile.
</action>

<onComplete>
- Summarize the `test.md` verdict (PASS/FAIL), the commits made, and the changed documents.
- Do **not** push.
- If the human has feedback, point them to `/scrumai.refine`.
</onComplete>
