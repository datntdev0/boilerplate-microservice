---
name: utilities
description: "Use for general utility actions: 
    1. Execute nswag commands to generate TypeScript clients for Angular from OpenAPI specifications.
    2. Execute nswag commands to generate C# clients for inter-communication from OpenAPI specifications.
    3. Generate new database migration and update database.
    4. Squash Migrations, Reset all database."
user-invocable: false
---

# SKILL - General Utilities

This skill set is designed to ONLY perform:
  1. Execute nswag commands to generate TypeScript clients for Angular from OpenAPI specifications.
  2. Execute nswag commands to generate C# clients for inter-service communication from OpenAPI specifications.

## Available Skills and Instructions

### Generate TypeScript Client for Angular

1. **Verify OpenAPI specification file**: Ensure the target microservice exposes its OpenAPI specification:
    ```bash
   # To Run specific microservice
   dotnet run --project srcs/services/<service-name>/datntdev.Microservice.Srv.<ServiceName>.csproj
   
   # Or run all microservices with Aspire
   dotnet run --project srcs/infra/Aspire/datntdev.Microservice.Infra.Aspire.csproj
   ```

2. **Fetch OpenAPI specification**: Ensure the OpenAPI specification is accessible.
    - The OpenAPI specification can be found at `http://localhost:<port>/swagger/v1/swagger.json`
    - Ports for microservices:
        - Identity Service: 7003
        - Admin Service: 7039
        - Notify Service: 7285
        - Payment Service: 7067
  
3. **Run nswag command**: Execute the `npm run nswag` command at Angular directory to generate TypeScript client
    - The command will read the OpenAPI specification from the url and generate TypeScript client code in `srcs\apps\Angular\src\shared\proxies` directory.

4. **Verify generated code**: Check the generated TypeScript client code for correctness and completeness.
    - Execute the `npm start` command to run the Angular application and ensure that the generated client code is working as expected.

### Generate CSharp Client for Intercommunication

0. **Ensure NSwag CLI is installed globally**: `dotnet tool install --global NSwag.ConsoleCore`

1. **Verify OpenAPI specification file**: Ensure the target microservice exposes its OpenAPI specification:
    ```bash
   # To Run specific microservice
   dotnet run --project srcs/services/<service-name>/datntdev.Microservice.Srv.<ServiceName>.csproj
   
   # Or run all microservices with Aspire
   dotnet run --project srcs/infra/Aspire/datntdev.Microservice.Infra.Aspire.csproj
   ```

2. **Fetch OpenAPI specification**: Ensure the OpenAPI specification is accessible.
    - The OpenAPI specification can be found at `http://localhost:<port>/swagger/v1/swagger.json`
    - Ports for microservices:
        - Identity Service: 7003
        - Admin Service: 7039
        - Notify Service: 7285
        - Payment Service: 7067

3. **Run nswag command**: Execute the `nswag run` command at `srcs/shared/Communication/HttpClients`.
    - The command will read the OpenAPI specification from the url and generate C# client code in `srcs\shared\Communication\HttpClients` directory.

4. **Update Generic Response Types**: 
    - **Important:** Since DTO generation is disabled, you need to update generic response types in the generated client code.
    - Replace NSwag-generated types with shared DTOs:
        ```csharp
        // Before (NSwag generated)
        PaginatedResultOfUserListDto
        ApiResponseOfUserProfileDto
        ResultOfPermissionDto

        // After (using shared DTOs)
        PaginatedResult<UserListDto>
        ApiResponse<UserProfileDto>
        Result<PermissionDto>

        // Patterns
        `PaginatedResultOf<T>` → `PaginatedResult<T>`
        `ApiResponseOf<T>` → `ApiResponse<T>`
        `ResultOf<T>` → `Result<T>`
        ```
    - Open the generated client file in `srcs/shared/Communication/HttpClients/` and search for these patterns to find all occurrences that need updating.

5. **Verify generated code**: Check the generated C# client code for correctness and completeness.
    - MUST build the solution to ensure that the generated client code does not cause any compilation errors.

### Generate new database migration and update database

0. **Ensure EF Core CLI tools are installed**: `dotnet tool install --global dotnet-ef --version 9.0.12`

1. **Create new migration**: This only apply for Relational databases, while the Admin and Notification microservices are using MongoDb this step is not applicable. To create a new migration, we are using Entity Framework Core CLI from the `Migrator` project directory:
    ```bash
    # Template
    cd srcs/infra/migrator
    dotnet ef migrations add <MigrationName> --output-dir Migrations/<MicroserviceName> --context <DbContextName>

    # Examples
    dotnet ef migrations add AddUserProfileTable --output-dir Migrations/App/Identity --context MicroserviceAppIdentityDbContext
    dotnet ef migrations add AddPaymentRefundColumn --output-dir Migrations/Srv/Payment --context MicroserviceSrvPaymentDbContext
    ```

2. **Verify generated migration**: Check the newly generated migration code for correctness and completeness. Migrations are organized by microservice in the Migrator project:
    - `Migrations/App/Identity/` - Application Identity context migrations
    - `Migrations/Srv/Identity/` - Identity service migrations
    - `Migrations/Srv/Payment/` - Payment service migrations

3. **Apply migration to database**: Run the Migrator project to apply the new migration to the database. The migrator will automatically apply all pending migrations to the respective databases for each microservice:
    ```bash
    dotnet run --project srcs/infra/Migrator/datntdev.Microservice.Infra.Migrator.csproj
    ```

4. **Verify database update**: Check the database to ensure that the new migration has been applied successfully: `dotnet ef migrations list --context <DbContextName>`

### Squash Migrations, Reset all database

> ⚠️ **WARNING:** This operation will delete all existing migration history and requires a clean database state. Use with caution—only in development environments before pushing to shared branches. You should clarify with the user before proceeding.

1. **Squash Migrations**: If there are too many migrations and you want to clean up the migration history, you can squash migrations by creating a new baseline migration that represents the current state of the database.
    - First, remove existing migrations in directory `Migrations/<MicroserviceName>/` with `rm -rf ./Migrations/<MicroserviceName>`
    - Second, drop the existing database to ensure a clean state: `dotnet ef database drop --context <DbContextName> --force`
    - Then, create a new baseline migration: `dotnet ef migrations add InitialCreate --output-dir Migrations/<MicroserviceName> --context <DbContextName>`
    - Finally, run the migrator to apply the new baseline migration to the database: `dotnet run --project srcs/infra/Migrator/datntdev.Microservice.Infra.Migrator.csproj`

2. **Reset all databases**: To reset all databases for all microservices, you can run the following command which will drop and recreate each database:
    ```bash
    # Down docker compose to stop and remove volumes for databases 
    docker compose -f deploy/dockercompose.local.infra.yml -p datntdev_microservices_infra down -v
    # Then rerun docker compose to recreate databases with fresh state
    docker compose -f deploy/dockercompose.local.infra.yml -p datntdev_microservices_infra up -d
    # Finally, run the migrator to apply all migrations to the new databases
    dotnet run --project srcs/infra/Migrator/datntdev.Microservice.Infra.Migrator.csproj
    ```

### Request access token from Identity Provider

1. Request access token from Identity Provider using password grant type for testing purposes (not recommended for production use):
    ```bash
    curl -X POST "https://localhost:7240/connect/token" \
        -H "Content-Type: application/x-www-form-urlencoded" \
        --data "grant_type=password&username=<username>&password=<password>" \
        --data "client_id=datntdev.Microservice.Public" \
        --insecure
    ```