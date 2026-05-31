---
name: scrumai-2-design-plan
description: Design an implementation plan for a new feature or bug fix based on the specification.
argument-hint: <specification_id> Provide the specification ID for which you want to design the implementation plan.
---

runSubagent("scrumai.leader.tech", {
  prompt: `Use EXACT taskflow = "design-plan" for {{chat_input}}`
})

STOP the execution after the taskflow is completed and return the result unless human request to continue.