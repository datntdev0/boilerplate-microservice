---
name: fe.write_tests
description: Implement frontend unit tests for Angular components and pages using Vitest.
---

# SKILL - Frontend Test Implementation

This prompt is designed to ONLY perform:
  1. Determine test cases for user requested components or pages.
  2. Implement frontend unit tests for Angular components and pages using Vitest.
  3. Provide the improved coverage report after implementing the tests.

## Workflows

0. **Clarification**:
    - If the user is not provide the list of components or pages, the agent will ask to clarify.
    - If the user request is out of scope, the agent will inform the user about the limitations.

1. **Provide Test Cases**:
    - Determine the test cases for the requested components or pages.
    - The number of test cases should be less as possible but still cover the key functionalities.
    - Provide the list of test cases to the user and ask for confirmation before implementation.

2. **Implement Tests**:
    - Execute command `npm run test:ci` to get current coverage report.
    - Review the source code of the specified component or page.
    - Implement unit tests using Vitest that cover the identified test cases.
    - Execute command `npm run test:ci` to ensure all changed tests pass and coverage is updated.

3. **Shows Results**: 
    - Parse the latest `coverage\coverage-summary.json` to identify component's coverage percent.
    - MUST provide the improvement result in following markdown format:
    ```markdown
    ### Frontend Coverage Improvement Summary:

    Lines       : {before}% → {after}%          (+{delta}%)
    Statements  : {before}% → {after}%          (+{delta}%)
    Functions   : {before}% → {after}%          (+{delta}%)
    Branches    : {before}% → {after}%          (+{delta}%)

    Passed      : {count} tests
    Failed      : {count} tests
    Duration    : {time}ms

    ### Detailed Test Cases Implemented:

      - path/to/source1.spec.ts
      - path/to/source2.spec.ts

    ### Test Cases Added ({count} total)
      ✓ should create the component
      ✓ should initialize with default values
      ✓ should handle user input correctly
      ✓ should emit events on action
      ✓ should handle error states
      ✓ should clean up resources on destroy
    ```

## References

- [Project Guidelines](../../../AGENTS.md)
- [Frontend Unit Test Conventions](../../instructions/fe.test.instructions.md)
