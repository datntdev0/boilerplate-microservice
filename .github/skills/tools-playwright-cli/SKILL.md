---
name: tools-playwright-cli
description: Use Playwright CLI to perform end-to-end testing on web applications.
user-invocable: false
---

<constraints>
- MUST use `playwright-cli` command in terminal.
- NEVER use the `npx playwright` command in terminal.
- NEVER use the playwright in the `e2e` directory.
- NEVER generate any test file or scripts to be executed.
- NEVER use typescript or javascript code to execute the test cases.
</constraints>

<instructions>
- Read `docs/1.requirements/application-sitemap.md` to understand the application structure and key pages.
</instructions>

<coreCommands>
  <command description="Open the browser in headless mode">
  ```bash
  playwright-cli open [url]
  ```
  </command>
  <command description="Open the browser in live view mode">
  ```bash
  playwright-cli open [url] --headed --profile ./.playwright-cli/profile
  ```
  </command>
  <command description="Snapshot the page as yml file">
  ```bash
  playwright-cli snapshot --filename=[path/to/save/snapshot_{title}.yml]
  ```
  </command>
  <command description="Capture a screenshot of the page">
  ```bash
  playwright-cli screenshot --filename=[path/to/save/screenshot_{title}.png]
  ```
  </command>
</coreCommands>

<examples>
  <example description="Open the browser and sign in to the application">
  ```bash 
  # 1. Open the browser in live view mode
  playwright-cli open https://example.com/login --headed --profile ./.playwright-cli/profile
  # 2. Snapshot the current page to determin is sign in required
  playwright-cli snapshot
  # 3. Determine the "Email" element reference and fill the defaultUsername
  playwright-cli fill [element_reference] "[username]"
  # 4. Determine the "Password" element reference and fill in the defaultPassword
  playwright-cli fill [element_reference] "[password]"
  # 5. Determine the "Sign In" button element reference and click it
  playwright-cli click [element_reference]
  ```
  </example>
  <example description="Open User Profile Menu popover from header">
  ```bash
  playwright-cli click "[data-testid='user-profile-menu']"
  ```
  </example>
</examples>