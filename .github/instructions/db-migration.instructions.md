---
description: "Use when: creating database migrations, updating database schema, running EF Core migrations, applying migrations with migrator console app, squashing migrations, rolling back migrations, data seeding"
---

# Database Migration Workflow

This instruction provides guidelines for performing database migrations using Entity Framework Core and the Migrator console application. It covers best practices, migration creation, application, and maintenance.

## Prerequisites

Ensure EF Core CLI tools are installed globally:

```bash
dotnet tool install --global dotnet-ef --version 9.0.12
```

## Step 1: Create a New Migration

Create a new migration using Entity Framework Core CLI from the `Migrator` project directory:

```bash
# Template
cd srcs/infra/migrator
dotnet ef migrations add <MigrationName> --output-dir Migrations/<MicroserviceName> --context <DbContextName>

# Examples
dotnet ef migrations add AddUserProfileTable --output-dir Migrations/App/Identity --context MicroserviceAppIdentityDbContext
dotnet ef migrations add AddPaymentRefundColumn --output-dir Migrations/Srv/Payment --context MicroserviceSrvPaymentDbContext
dotnet ef migrations add CreateNotificationIndexes --output-dir Migrations/Srv/Notify --context MicroserviceSrvNotifyDbContext
```

### Naming Convention
- Use PascalCase for migration names
- Be descriptive: `AddUserProfileTable`, `CreatePaymentIndexes`
- Avoid generic names like `Update1`

### Output Structure
Migrations are organized by microservice in the Migrator project:
- `Migrations/App/Identity/` - Application Identity context migrations
- `Migrations/Srv/Identity/` - Identity service migrations
- `Migrations/Srv/Payment/` - Payment service migrations
- `Migrations/Srv/Notify/` - Notify service migrations

## Step 2: Verify Migration File

Before applying, verify the generated migration file:

1. Locate the migration file in the appropriate directory
2. Review the SQL that will be executed:
   - Column additions/removals
   - Index creation/deletion
   - Constraint changes
3. Ensure `Down()` method exists for rollback capability
4. Check for any data loss implications

## Step 3: Apply the Migration

Apply the migration by running the Migrator console application:

```bash
# Navigate to Migrator project
cd srcs/infra/migrator

# Run the migrator (applies all pending migrations)
dotnet run
```

The Migrator application will:
- Apply all pending migrations to the database
- Execute data seeding if defined
- Update the `__EFMigrationsHistory` table

## Step 4: Verify Database Changes

Confirm the migration was applied successfully:

1. **Check migration history**:
   ```bash
   dotnet ef migrations list --context <DbContextName>
   ```

2. **Verify schema changes** in your database:
   - Connect to the database
   - Query the `__EFMigrationsHistory` table
   - Confirm your migration appears with status `Completed`

3. **Run tests** to ensure no breaking changes

## Advanced: Squash Migrations

> ⚠️ **WARNING:** This operation will delete all existing migration history and requires a clean database state. Use with caution—only in development environments before pushing to shared branches.

Use this to consolidate multiple migrations into a single migration:

### 1. Remove Existing Migrations
```bash
cd srcs/infra/migrator
rm -rf ./Migrations/<MicroserviceName>
```

Example:
```bash
rm -rf ./Migrations/Srv/Identity
```

### 2. Drop the Database
```bash
dotnet ef database drop --context <DbContextName> --force
```

Example:
```bash
dotnet ef database drop --context MicroserviceSrvIdentityDbContext --force
```

### 3. Create New Initial Migration
```bash
dotnet ef migrations add <InitialMigrationName> --output-dir Migrations/<MicroserviceName> --context <DbContextName>
```

Example:
```bash
dotnet ef migrations add InitialCreate --output-dir Migrations/Srv/Identity --context MicroserviceSrvIdentityDbContext
```

### 4. Apply the Migration
```bash
dotnet run
```

## Best Practices

### Planning
- ✅ Plan migrations before making code changes
- ✅ One logical change per migration
- ✅ Keep migration names descriptive

### Safety
- ✅ Always test migrations locally first
- ✅ Verify `Down()` method for rollback capability
- ✅ Run full test suite after applying migrations
- ✅ Never modify migration files after they're shared

### Data
- ✅ Include data seeding in migrations when needed
- ✅ Test with realistic data volumes
- ✅ Plan for zero-downtime deployments with large tables

### Review
- ✅ Review generated migration files before applying
- ✅ Check for unwanted schema changes
- ✅ Verify performance impact of indexes/constraints

## Troubleshooting

| Issue | Solution |
|-------|----------|
| **"dotnet-ef" command not found** | Install EF Core tools: `dotnet tool install --global dotnet-ef --version 9.0.12` |
| **Migration file already exists** | Use a different migration name or delete the conflicting file |
| **Database already has migration applied** | The `__EFMigrationsHistory` table prevents re-applying migrations |
| **Migration fails with SQL error** | Review the `Up()` method SQL. Check database constraints and existing data compatibility |
| **Can't create migration (no changes detected)** | Ensure model changes are made in the DbContext or entity classes |
| **Want to rollback a migration** | Remove migration from history and drop affected tables, or create a new migration to undo changes |

## Related Documentation

- [EF Core Migrations Documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [EF Core CLI Reference](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [Database Design Guidelines](../../docs/2.architecture/01.high-level-architecture.md)
