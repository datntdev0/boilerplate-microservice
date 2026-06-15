---
name: tools-playwright
description: Drive a real browser interactively to manually verify a just-built web/UI feature during development, capturing snapshots/screenshots as evidence. Use during /scrumai.implement.verify when a feature has UI or web behavior. Hands-on manual testing via the globally installed playwright-cli — NOT the e2e/ regression suite.
allowed-tools: Bash(playwright-cli:*), Read, Write
user-invocable: false
---

# tools-playwright

Interactive, manual browser testing for the **verification phase**. The agent drives a real
browser with the **`playwright-cli`** command — navigate, snapshot, click, fill, screenshot — to
confirm a freshly implemented feature behaves per its `spec.md` acceptance scenarios.

> This is **development-time manual testing**, not regression automation. Do **not** write or run
> specs in `e2e/`. The `e2e/` project is a separate, human-owned regression suite authored only
> after a feature has stabilized.

## Mechanism — playwright-cli

`playwright-cli` is assumed to be **installed globally** and on `PATH`. Always invoke it directly
as `playwright-cli ...` — do **not** use the Playwright installed in `e2e/` (that project is the
regression suite and is independent of this manual-testing flow). It keeps browser state on disk
and prints a snapshot after each command, so it's token-light.

```bash
playwright-cli --version   # confirm it's available
```

If the command is missing, install it globally once: `npm install -g @playwright/cli@latest`
(then `playwright-cli install chromium` if browsers aren't present). The tool also ships a full
command reference skill (`playwright-cli`'s own SKILL.md) covering tabs, storage, network mocking,
and tracing — consult it for commands beyond the core set below.

### Core commands you'll use

```bash
playwright-cli open http://localhost:4200   # launch browser + navigate
playwright-cli snapshot                      # get the accessibility tree with element refs (e1, e2, …)
playwright-cli click e15                      # act on a ref from the snapshot
playwright-cli fill e5 "user@example.com"    # type into a field (add --submit to press Enter)
playwright-cli screenshot --filename=...      # save evidence
playwright-cli console                        # check for page errors
playwright-cli close
```

Interact using **refs** from the latest `snapshot`, or CSS / role / test-id locators.

## App endpoints (local)

| What | URL |
|------|-----|
| Angular SPA | http://localhost:4200 |
| Identity provider (login) | https://localhost:7240 |

Start the app first — full stack via Aspire (`dotnet run --project ./srcs/infra/Aspire/...`) or the
SPA alone via `npm start` in `srcs/apps/Angular`. Confirm the page loads before driving it.

## Procedure

1. **Start the app** and confirm the target URL responds.
2. `playwright-cli open http://localhost:4200`, then `playwright-cli snapshot`.
3. **Log in** (most pages are protected): the SPA redirects to the identity provider
   (`/auth/signin`); `fill` the seeded admin credentials (from `appsettings.Common.json` seed
   config) and submit, landing back in the app. Tip: `state-save auth.json` once, then
   `state-load auth.json` to skip re-login on later runs.
4. **Walk each acceptance scenario** from `spec.md`: `snapshot` → act (`click`/`fill`/`select`) →
   `snapshot` again and confirm the expected state appeared. Use `console` / `requests` to catch errors.
5. **Capture evidence** at each key state:
   ```bash
   playwright-cli screenshot --filename=../.scrumai/features/<name>/evidence/<scenario>-<step>.png
   ```
6. `playwright-cli close` when done.
7. **Record PASS/FAIL per scenario** with a one-line note and the evidence filename.

## Optional — human-in-the-loop UI review

For design/UX feedback, `playwright-cli show --annotate` lets the user draw on the live page and
type notes; you receive the annotated screenshot + snapshot. Use when the spec calls for human sign-off.

## Reporting

Hand back a PASS/FAIL per acceptance scenario, each linked to its evidence file, plus the browser
and URL used. `/scrumai.implement.verify` records this in `test.md`.
