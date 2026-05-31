---
name: scrumai-1-report-issue
description: Verify and report an issue into specification.md file.
argument-hint: <issue_id> Describe the issue you want to report.
---

runSubagent("scrumai.leader.test", {
  prompt: `Use EXACT taskflow = "report-issue" for {{chat_input}}`
})

STOP the execution after the taskflow is completed and return the result unless human request to continue.