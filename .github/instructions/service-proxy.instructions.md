---
description: "Use when: generating HTTP client, NSwag client generation, creating service-to-service communication clients, updating API clients from OpenAPI specs, inter-microservice communication setup"
---

# Generate HTTP Client for Inter-Microservice Communication

This instruction provides the detailed procedure for generating C# HTTP clients based on OpenAPI specifications using NSwag. These clients facilitate type-safe communication between microservices in the boilerplate architecture.

## Prerequisites

Ensure NSwag CLI is installed globally:

```bash
dotnet tool install --global NSwag.ConsoleCore
```

## Step 1: Verify OpenAPI Specification

Ensure the target microservice exposes its OpenAPI specification:

1. **Run the microservice** (choose one):
   ```bash
   # Run specific microservice
   dotnet run --project srcs/services/<service-name>/datntdev.Microservice.Srv.<ServiceName>.csproj
   
   # Or run all microservices with Aspire
   dotnet run --project srcs/infra/Aspire/datntdev.Microservice.Infra.Aspire.csproj
   ```

2. **Access the OpenAPI specification** in your browser:
   - Navigate to: `https://localhost:<port>/openapi/v1.json`
   - **Common ports:**
     - Identity Service: 7003
     - Admin Service: 7039
     - Notify Service: 7285
     - Payment Service: 7067
   - Verify the JSON specification loads correctly

## Step 2: Run NSwag Generation

1. Navigate to the NSwag configuration directory:
   ```bash
   cd srcs/shared/Communication/HttpClients
   ```

2. Run the NSwag command:
   ```bash
   # Generate all configured clients
   nswag run
   
   # Or generate specific client
   nswag run SrvIdentity.nswag
   nswag run SrvAdmin.nswag
   nswag run SrvNotify.nswag
   nswag run SrvPayment.nswag
   ```

## Step 3: Update Generic Response Types

**Important:** Since DTO generation is disabled, manually update generic response types in the generated client code.

Replace NSwag-generated types with shared DTOs:

```csharp
// Before (NSwag generated)
PaginatedResultOfUserListDto
ApiResponseOfUserProfileDto
ResultOfPermissionDto

// After (using shared DTOs)
PaginatedResult<UserListDto>
ApiResponse<UserProfileDto>
Result<PermissionDto>
```

**Common patterns to update:**
- `PaginatedResultOf<T>` → `PaginatedResult<T>`
- `ApiResponseOf<T>` → `ApiResponse<T>`
- `ResultOf<T>` → `Result<T>`

Open the generated client file in `srcs/shared/Communication/HttpClients/` and search for these patterns to find all occurrences that need updating.

## Step 4: Verify Generation

Validate the generated client code:

1. Locate the generated file in `srcs/shared/Communication/HttpClients/`
2. Verify that:
   - All methods match the API endpoints from the OpenAPI spec
   - Response types are correctly using shared DTOs (not NSwag-generated wrappers)
   - Using statements reference proper namespaces for shared DTOs
   - No compilation errors when building the solution

3. **Build and test**:
   ```bash
   dotnet build srcs/shared/Communication/datntdev.Microservice.Shared.Communication.csproj
   ```

## Configuration Files

NSwag configuration files (`.nswag`) are located in:
```
srcs/shared/Communication/HttpClients/
```

Each service has its own configuration following the naming pattern:
- `SrvIdentity.nswag`
- `SrvAdmin.nswag`
- `SrvNotify.nswag`
- `SrvPayment.nswag`

## Troubleshooting

| Issue | Solution |
|-------|----------|
| **OpenAPI spec not accessible** | Ensure target microservice is running. Check port number matches configuration. Verify route is `/openapi/v1.json` |
| **Generated code has compilation errors** | Check that shared DTOs are imported. Update generic types as described in Step 3. Verify NSwag version compatibility |
| **Client doesn't reflect latest API changes** | Restart the target microservice. Clear any cached OpenAPI specs. Re-run NSwag generation |
| **"nswag" command not found** | Install NSwag globally: `dotnet tool install --global NSwag.ConsoleCore` |

## Related Documentation

- [Application Service Design](../../docs/2.architecture/03.app-service-design.md)
- [OpenAPI/Swagger Specification](https://swagger.io/specification/)
