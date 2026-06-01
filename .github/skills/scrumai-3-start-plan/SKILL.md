---
name: scrumai-3-start-plan
description: Start implementation for a new feature or bug fix based on the design plan.
argument-hint: <specification_id> Provide the specification ID for which you want to start the implementation.
---

runSubagent("scrumai.leader.tech", {
  prompt: `Use EXACT taskflow = "start-plan" for {{chat_input}}`
})

STOP the execution after the taskflow is completed and return the result unless human request to continue.