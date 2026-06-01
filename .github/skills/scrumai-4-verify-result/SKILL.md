---
name: scrumai-4-verify-result
description: Verify the implementation result of a feature or bug fix based on the verification plan.
argument-hint: <specification_id> Provide the specification ID for which you want to verify the implementation result.
---

runSubagent("scrumai.tester", {
  prompt: `Use EXACT taskflow = "verify-implementation" for {{chat_input}}`
})

STOP the execution after the taskflow is completed and return the result unless human request to continue.