---
name: utilities
description: Use for general utility functions, helper methods, or any actions that repeatly occur across the codebase
user-invocable: false
---

# SKILL - General Utilities

This skill set is designed to ONLY perform:
  1. Execute nswag commands to generate TypeScript clients for Angular from OpenAPI specifications.
  2. Execute nswag commands to generate C# clients for inter-service communication from OpenAPI specifications.

## Workflows

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
    - **Important:** Since DTO generation is disabled, manually update generic response types in the generated client code.
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

4. **Verify generated code**: Check the generated C# client code for correctness and completeness.
    - Build the solution to ensure that the generated client code does not cause any compilation errors.