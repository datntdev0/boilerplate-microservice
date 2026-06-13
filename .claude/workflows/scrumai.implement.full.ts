export const meta = {
  name: 'scrumai.implement.full',
  description: 'From a ready spec, run Design → Implement → Verify autonomously, then stop for human review.',
  phases: [
    { title: 'Design', detail: 'spec.md → design.md + plan.md (incl. test cases)' },
    { title: 'Implement', detail: 'work plan.md, internal review, local commits' },
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

// Design must return the resolved feature folder so later phases target the same one.
const DESIGN_RESULT = {
  type: 'object',
  additionalProperties: false,
  required: ['feature', 'summary'],
  properties: {
    feature: { type: 'string', description: 'Resolved feature folder, e.g. .scrumai/features/001-user-auth' },
    summary: { type: 'string', description: 'One-paragraph summary of the design.' },
  },
}

phase('Design')
const design = await agent(
  `Run Phase ② Design by following .claude/commands/scrumai.implement.design.md.\n` +
  (target ? `Feature folder: ${target}.` : `No folder given — resolve the most recent .scrumai/features/<NNN>-<name>/.`) +
  `\nReturn the resolved feature folder path and a one-paragraph summary.`,
  { label: 'design', schema: DESIGN_RESULT },
)

const feature = (design && design.feature) || target
log(`Design complete for ${feature}`)

phase('Implement')
const implement = await agent(
  `Run Phase ③ Implement for feature ${feature} by following .claude/commands/scrumai.implement.start.md.\n` +
  `Return a summary of the changes and the commits made.`,
  { label: 'implement' },
)

phase('Verify')
const verify = await agent(
  `Run Phase ④ Verify for feature ${feature} by following .claude/commands/scrumai.implement.verify.md.\n` +
  `Return the PASS/FAIL verdict and a short summary.`,
  { label: 'verify' },
)

log(`implement.full complete for ${feature} — stopping for human review.`)
return { feature, design, implement, verify }
