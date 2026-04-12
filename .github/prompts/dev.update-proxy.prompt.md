---
description: "Use when: generating or updating HTTP client for inter-microservice communication using NSwag, creating service-to-service API client, updating client from OpenAPI spec"
argument-hint: "Optional: service name (identity, admin, notify, payment) or leave blank for interactive guidance"
---

# Generate/Update HTTP Client with NSwag

Generate a type-safe C# HTTP client for inter-microservice communication from the target service's OpenAPI specification.

**This task will:**
- Verify the target microservice's OpenAPI specification is accessible
- Run NSwag to generate the HTTP client
- Update generic response types to use shared DTOs
- Validate the generated code

Provide the target service name (e.g., `identity`, `admin`, `notify`, `payment`), or leave blank for step-by-step guidance.

Refer to [proxy-client.instructions.md](../instructions/service-proxy.instructions.md) for detailed procedures.
