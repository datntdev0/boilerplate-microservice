---
description: Any conversation with GitHub Copilot should start here. This file provides an overview of the project architecture, instructions, and constraints for all tasks.
applyTo: '**'
---

# GitHub Copilot Instructions - entry point for all project instructions

## Instructions and Constraints

- DO NOT violate or skip any following instructions or constraints.
- DO NOT violate or skip any insutructions or constraints in related instruction files.
- ALWAYS read the instruction files related to the task FIRSTLY, before reading to codebase.
- ALWAYS read to the backlinks of the relevant instructions to have a complete understanding.

## Project Structure and Architecture

```
├── .github/                    # GitHub-specific files
├── docs/                       # Documentation
├── srcs/
│   ├── apps/                   # API layer with controllers and routes
│   │   ├── Angular/            # Angular frontend application 
│   │   └── Identity/           # Blazor for Identity Provider
│   ├── services/               # Microservices for different domains
│   │   ├── admin/              # Admin service for tenant management
│   │   ├── identity/           # Identity service for user and role management
│   │   ├── notify/             # Notification service for sending emails and messages
│   │   └── payment/            # Payment service for handling transactions
│   ├── infrastructure/         # External services, databases, and repositories
│   │   ├── aspire/             # Aspire for database access and migrations
│   │   ├── gateway/            # API Gateway for routing and load balancing
│   │   └── migrator/           # Database migration tools and scripts
│   └── shared/                 # Common utilities, constants, and helpers
├── tests/                      # Microservice integration tests
├── e2e/                        # End-to-end Playwright tests for the entire system
├── deploy/                     # Docker, scripts, and configuration files for deployment
└── README.md                   # Project overview and setup instructions
```

## Technical Stack:

1. Frontend - Angular for Web SPA.
2. Backend - .NET 9 for microservices.
3. Gateway - YARP for API Gateway.
4. Identity - Blazor with OpenIddict for Identity Provider.
5. Orchestration - .NET Aspire for local development and testing.
6. Databases - SQL Server for relational data.
7. Databases - MongoDB for document storage.
8. Migrator - .NET Console app for database migrations.
9. Testing - MSTest for unit and integration tests.
10. Testing - Playwright for end-to-end tests.