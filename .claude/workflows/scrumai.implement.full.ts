export const meta = {
  name: 'scrumai.implement.full',
  description: 'From a ready spec, run Design → Implement → Verify autonomously, then stop for human review.',
  phases: [
    { title: 'Design', detail: 'spec.md → design.md + tasks in checklist.md (incl. test cases)' },
    { title: 'Implement', detail: 'work checklist ③ tasks, internal review, local commits' },
    { title: 'Verify', detail: 'review + unit/manual tests → test.md (PASS/FAIL)' },
  ],
}

// This script only ORCHESTRATES. Each phase's actual procedure lives in its command file
// (.claude/commands/scrumai.implement.*.md), the scrumai-conventions skill, the templates,
// and the agent definitions — the single source of truth. Don't restate those rules here;
// keep the prompts as thin delegations so there's nothing to drift.

// `args` may be a string (feature folder) or { feature }. Empty = let Design resolve the latest.
const target =
  (args && typeof args === 'object' && args.feature) ? args.feature :
  (typeof args === 'string' ? args : '')

phase('Design')
const design = await agent(
  `Run Phase ② Design by following .claude/commands/scrumai.design.md.\nFeature folder: ${target}. `,
  { label: 'design' },
)

log(`Design complete for ${target}`)

phase('Implement')
const implement = await agent(
  `Run Phase ③ Implement by following .claude/commands/scrumai.start.md.\nFeature folder: ${target}. ` +
  `Return the failed tasks, if any, and a short summary of the implementation progress.`,
  { label: 'implement' },
)

log(`Implement complete for ${target}`)

phase('Verify')
const verify = await agent(
  `Run Phase ④ Verify by following .claude/commands/scrumai.verify.md.\nFeature folder: ${target}. ` +
  `Return the PASS/FAIL verdict and a short summary.`,
  { label: 'verify' },
)

log(`implement.full complete for ${target} — stopping for human review.`)
return { feature: target, design, implement, verify }
