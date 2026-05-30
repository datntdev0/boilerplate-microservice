---
name: scrumai-1-refine-story
description: Refine the requirement description into specification.md file.
argument-hint: <feature_id> Describe the feature you want to implement.
---

runSubagent("scrumai.product-owner", {
  prompt: `Use EXACT taskflow = "analyze-requirements" for {{chat_input}}`
})

STOP the execution after the taskflow is completed and return the result unless human request to continue.