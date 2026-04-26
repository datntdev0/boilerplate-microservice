---
name: fe.analysis_tests
description: Analysis and provide insight for frontend test coverage and suggest test cases to implement.
---

# PROMPT - Frontend Test Analysis and Suggestions

This prompt is designed to ONLY perform:
  1. Run the frontend unit test and generate coverage report.
  2. Analyze the coverage report and provide insights to improve coverage.
  3. Suggest specific test cases to implement based on the analysis.

## Workflows

0. **Clarification**:
    - If the user request is out of scope, the agent will inform the user about the limitations.

1. **Analyze Coverage**: If the user requests to analyze test coverage, the agent will:
    - Execute command `npm run test:ci` to run coverage report for Angular application.
    - Parse the `coverage\coverage-summary.json` file to identify component's coverage percent.

2. **Show Insights**: MUST provide insights in following markdown format:
    - Frontend Coverage Analysis Summary
    - Top 10 Priority Files by Highest Uncovered Lines
    - Estimated Coverage After Testing Files
    ```markdown
    ### Frontend Coverage Analysis Summary

    Total Overall Coverage      : {percentage}%
    Total Files Analyzed        : {count}
    Test Failed Files           : {count}
    Uncovered Lines Remaining   : {count}

    ### Top 10 Priority Files by Highest Uncovered Lines

    File Path             | Coverage %   | Uncovered Lines   
    --------------------- | ------------ | ------------------
    {relative-file-path1} | {coverage1}% | {uncovered-lines1}
    {relative-file-path2} | {coverage2}% | {uncovered-lines2}
    {relative-file-path3} | {coverage3}% | {uncovered-lines3}
    {relative-file-path4} | {coverage4}% | {uncovered-lines4}

    ### Estimated Coverage After Testing Files

    Top 3 files:        {from}% → {to}% (+{increase}% improvement)
    Top 5 files:        {from}% → {to}% (+{increase}% improvement)
    Top 10 files:       {from}% → {to}% (+{increase}% improvement)
    ```

## References

- [Project Guidelines](../../../AGENTS.md)
- [Frontend Unit Test Conventions](../../instructions/fe.test.instructions.md)
