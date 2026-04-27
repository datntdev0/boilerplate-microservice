---
name: be.develop.overview.instructions
description: "Use when developing backend, this instruction file contains overview of backend conventions, project structure, coding style, and best practices for the .NET microservices."
applyTo: "srcs/**/*.cs"
---

# INSTRUCTION - Backend Development Overview Instructions

## Instructions & Constraints

- DO NOT manually edit or create HttpClients in `srcs\shared\Communication\HttpClients`.
- MUST use the SKILL [Generate CSharp Client for Intercommunication](../skills/utilities/SKILL.md).