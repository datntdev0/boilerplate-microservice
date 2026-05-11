---
name: be.develop.overview.instructions
description: "Use when developing backend, this instruction file contains overview of backend conventions, project structure, coding style, and best practices for the .NET microservices."
applyTo: "srcs/**/*.cs"
---

# INSTRUCTION - Backend Development Overview Instructions

## Instructions & Constraints

- MUST perform the `dotnet build` command in the root directory to ensure all projects are built successfully after your changes.

### Generate C# Clients for Inter-Service Communication

- DO NOT manually edit or create HttpClients in `srcs\shared\Communication\HttpClients`.
- MUST use the SKILL [Generate CSharp Client for Intercommunication](../skills/utilities/SKILL.md).