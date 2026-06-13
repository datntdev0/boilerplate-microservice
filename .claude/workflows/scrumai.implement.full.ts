export const meta = {
  name: 'scrumai.implement.full',
  description: 'From a ready spec, run Design → Implement → Verify autonomously, then stop for human review.',
  phases: [
    { title: 'Design', detail: 'spec.md → design.md + plan.md (incl. test cases)' },
    { title: 'Implement', detail: 'work plan.md, internal review, local commits' },
    { title: 'Verify', detail: 'review + unit/manual tests → test.md (PASS/FAIL)' },
  ],
}

// `args` may be a string (feature folder) or { feature }. Empty = let Design resolve the latest.
const target =
  (args && typeof args === 'object' && args.feature) ? args.feature :
  (typeof args === 'string' ? args : '')

const DESIGN_RESULT = {
  type: 'object',
  additionalProperties: false,
  required: ['feature', 'summary'],
  properties: {
    feature: { type: 'string', description: 'Resolved feature folder, e.g. .scrumai/features/001-user-auth' },
    summary: { type: 'string', description: 'One-paragraph summary of the design.' },
  },
}

// ── Phase ② Design ───────────────────────────────────────────────────────────
phase('Design')
const design = await agent(
  [
    'You are running Phase ② Design of the ScrumAI workflow.',
    target ? `Feature folder: ${target}` : 'Resolve the most recent .scrumai/features/<NNN>-<name>/ folder.',
    'Load the `scrumai-conventions` skill. Read .claude/commands/scrumai.implement.design.md,',
    '.claude/memory/constitution.md, and the feature\'s spec.md (confirm it has no open clarifications).',
    'Design the solution (affected services/modules, DDD/SOLID, modular BaseModule DI,',
    'data/contract/migration impacts, risks) and define test cases — unit tests AND manual tests.',
    'Write design.md from .claude/templates/design.md (including the Test Cases section) and',
    'plan.md from .claude/templates/plan.md into the feature folder. Do NOT edit source code in this phase.',
    'Return the resolved feature folder path and a one-paragraph summary.',
  ].join('\n'),
  { label: 'design', schema: DESIGN_RESULT },
)

const feature = (design && design.feature) || target
log(`Design complete for ${feature}`)

// ── Phase ③ Implement ────────────────────────────────────────────────────────
phase('Implement')
const implement = await agent(
  [
    `You are running Phase ③ Implement of the ScrumAI workflow for feature ${feature}.`,
    'Load the `scrumai-conventions` skill and follow .claude/commands/scrumai.implement.start.md.',
    'Ensure you are on a feature branch (not `main`). Work plan.md to completion: implement each',
    'task following repo idioms and the modular DI system, build (`dotnet build` / `ng build`) to',
    'confirm it compiles, commit locally with conventional messages, and check tasks off in plan.md.',
    'Then run an internal code review applying .claude/agents/scrumai.reviewer.md and',
    '.claude/memory/constitution.md against spec.md and design.md; fix the findings and commit.',
    'Do NOT push. Return a summary of the changes and the commits made.',
  ].join('\n'),
  { label: 'implement' },
)

// ── Phase ④ Verify ───────────────────────────────────────────────────────────
phase('Verify')
const verify = await agent(
  [
    `You are running Phase ④ Verify of the ScrumAI workflow for feature ${feature}.`,
    'Load the `scrumai-conventions` skill and follow .claude/commands/scrumai.implement.verify.md.',
    'Code review: apply .claude/agents/scrumai.reviewer.md (enforces the constitution).',
    'Testing: run the design\'s test cases — unit (`dotnet test --settings .runsettings`,',
    '`npm run test:ci`) and, if the feature has UI/web behavior, manual browser testing via the',
    '`tools-playwright` skill (globally installed playwright-cli). Capture evidence into the',
    'feature\'s evidence/ folder.',
    'Write test.md from .claude/templates/test.md with findings, results, evidence links, and a',
    'PASS/FAIL verdict. Do NOT push. Return the verdict and a short summary.',
  ].join('\n'),
  { label: 'verify' },
)

log(`implement.full complete for ${feature} — stopping for human review.`)
return { feature, design, implement, verify }
