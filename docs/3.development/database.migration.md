# Database Migration Guide

This document provides guidelines for performing database migrations in our project. It covers best practices, tools, and steps to ensure smooth transitions between database schema versions.

## Overview

The `srcs/infra/Migrator` project is the centralized migration runner. It handles:

- **SQL Server** (EF Core migrations): `App.Identity`, `Srv.Identity`, `Srv.Payment`
- **MongoDB** (schema creation via `EnsureCreated`): `Srv.Admin`, `Srv.Notify`

MongoDB contexts do not use EF Core migrations — they are managed automatically at startup. Only SQL Server contexts require migration files.

## DbContext Reference

| Context Name | Migration Folder | Connection String Key |
|---|---|---|
| `MicroserviceAppIdentityDbContext` | `Migrations/App/Identity` | `App.Identity` |
| `MicroserviceSrvIdentityDbContext` | `Migrations/Srv/Identity` | `Srv.Identity` |
| `MicroserviceSrvPaymentDbContext` | `Migrations/Srv/Payment` | `Srv.Payment` |
| `MicroserviceSrvAdminDbContext` | _(none — MongoDB)_ | `Srv.Admin` |
| `MicroserviceSrvNotifyDbContext` | _(none — MongoDB)_ | `Srv.Notify` |

## Prerequisites

Install the EF Core CLI tools globally:

```bash
dotnet tool install --global dotnet-ef --version 9.0.12
```

## Updating the Database Schema

### Step 1 — Create a new migration

Run the following command from the `srcs/infra/Migrator` project directory:

```bash
dotnet ef migrations add <MigrationName> --output-dir Migrations/<MicroserviceName> --context <DbContextName>
```

**Examples for each SQL Server context:**

```bash
# App Identity (OpenIddict / login UI)
dotnet ef migrations add <MigrationName> --output-dir Migrations/App/Identity --context MicroserviceAppIdentityDbContext

# Service Identity (users, roles, tenants)
dotnet ef migrations add <MigrationName> --output-dir Migrations/Srv/Identity --context MicroserviceSrvIdentityDbContext

# Service Payment
dotnet ef migrations add <MigrationName> --output-dir Migrations/Srv/Payment --context MicroserviceSrvPaymentDbContext
```

### Step 2 — Apply migrations

Run the `Migrator` project. It will:

1. Apply pending EF Core migrations for all SQL Server contexts
2. Call `EnsureCreated` for MongoDB contexts
3. Run all seeders (default tenant, OpenIddict apps, roles, admin user)

```bash
dotnet run --project srcs/infra/Migrator
```

> You can also launch it via the Aspire orchestrator (`srcs/infra/Aspire`), which starts the Migrator together with all other services.

## Squashing Migrations

Use this procedure when accumulated migrations become difficult to maintain and you want to replace them with a single baseline snapshot. Perform this for one context at a time.

> **Warning:** Squashing drops and recreates the target database. Only do this in a development environment or when you have a full backup of the target database.

Run all commands from the `srcs/infra/Migrator` directory.

### App.Identity

```bash
rm -rf ./Migrations/App/Identity
dotnet ef database drop --context MicroserviceAppIdentityDbContext --force
dotnet ef migrations add InitialDatabase --output-dir Migrations/App/Identity --context MicroserviceAppIdentityDbContext
```

### Srv.Identity

```bash
rm -rf ./Migrations/Srv/Identity
dotnet ef database drop --context MicroserviceSrvIdentityDbContext --force
dotnet ef migrations add InitialCreate --output-dir Migrations/Srv/Identity --context MicroserviceSrvIdentityDbContext
```

### Srv.Payment

```bash
rm -rf ./Migrations/Srv/Payment
dotnet ef database drop --context MicroserviceSrvPaymentDbContext --force
dotnet ef migrations add InitialDatabase --output-dir Migrations/Srv/Payment --context MicroserviceSrvPaymentDbContext
```

### Conclusion

After squashing, re-apply by running the Migrator project to recreate the database with the new baseline migration and seed data.


