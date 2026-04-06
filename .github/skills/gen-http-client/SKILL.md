---
name: gen-http-client
description: 'Generate C# HTTP clients for inter-microservice communication using NSwag. Use when: generating HTTP client, NSwag client generation, creating service-to-service communication clients, updating API clients from OpenAPI specs, inter-microservice communication setup.'
argument-hint: 'Optional: specify target service name (e.g., identity, admin, notify, payment)'
---

# Generate HTTP Client for Inter-Microservice Communication

This skill helps you generate C# HTTP clients based on OpenAPI specifications using NSwag. These clients facilitate type-safe communication between microservices in the boilerplate architecture.

## When to Use

- Creating new HTTP clients for service-to-service communication
- Updating existing clients after API changes
- Setting up communication between microservices
- Generating type-safe API clients from OpenAPI/Swagger specs

## Prerequisites

Ensure NSwag CLI is installed globally:

```bash
dotnet tool install --global NSwag.ConsoleCore
```

## Procedure

### 1. Verify OpenAPI Specification

Ensure the target microservice exposes its OpenAPI specification:

- Run the specific microservice (e.g., Identity, Admin, Notify, Payment) or you can run all with Aspire project: 

```bash
# To run specific microservice
dotnet run --project <path-to-microservice-csproj>
# To run all microservices with Aspire project
dotnet run --project srcs/infra/Aspire/datntdev.Microservice.Infra.Aspire.csproj
```
- Navigate to the OpenAPI endpoint in your browser: `https://localhost:<port>/openapi/v1.json`
- Common ports:
  - Identity Service: 7003
  - Admin Service: 7004
  - Notify Service: 7005
  - Payment Service: 7006
- Verify the JSON specification loads correctly

### 2. Run NSwag Generation

Navigate to the NSwag configuration directory:

```bash
cd srcs/shared/Communication/HttpClients
```

Run the NSwag command:

```bash
# Generate all configured clients
nswag run

# Generate specific client
nswag run <ServiceName>.nswag
```

Example for specific services:
- `nswag run SrvIdentity.nswag`
- `nswag run SrvAdmin.nswag`
- `nswag run SrvNotify.nswag`
- `nswag run SrvPayment.nswag`

### 3. Update Generic Response Types

**Important:** Since DTO generation is disabled, manually update generic response types in the generated client code.

Replace NSwag-generated types with shared DTOs:

```csharp
// Before (NSwag generated)
PaginatedResultOfUserListDto

// After (using shared DTOs)
PaginatedResult<UserListDto>
```

Common patterns to update:
- `PaginatedResultOf<T>` → `PaginatedResult<T>`
- `ApiResponseOf<T>` → `ApiResponse<T>`
- `ResultOf<T>` → `Result<T>`

### 4. Verify Generation

Check the generated client file:
- Located in `srcs/shared/Communication/HttpClients/`
- Verify methods match the API endpoints
- Ensure proper using statements for shared DTOs

## Configuration Files

NSwag configuration files (`.nswag`) are located in:
```
srcs/shared/Communication/HttpClients/
```

Each service should have its own configuration file following the naming pattern:
- `SrvIdentity.nswag`
- `SrvAdmin.nswag`
- `SrvNotify.nswag`
- `SrvPayment.nswag`

## Troubleshooting

**OpenAPI spec not accessible:**
- Ensure the target microservice is running
- Check the port number in your configuration
- Verify the route matches `/openapi/v1.json`

**Generated code has compilation errors:**
- Check that shared DTOs are properly referenced
- Update generic types as described in Step 3
- Ensure NSwag version is compatible

**Client not reflecting latest API changes:**
- Restart the target microservice
- Clear any cached OpenAPI specs
- Re-run the NSwag generation command

## Related Documentation

- [NSwag Generate HTTP Client](../../../docs/3.development/04.nswag-generate-http-client.md)
- [Application Service Design](../../../docs/2.architecture/03.app-service-design.md)
