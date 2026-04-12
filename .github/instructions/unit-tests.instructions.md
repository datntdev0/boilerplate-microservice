---
name: "Unit Testing Guidelines"
applyTo: "tests/**/*Tests.cs,srcs/apps/Angular/**/*.spec.ts"
description: "Use when: running unit tests, writing test cases, designing test coverage, debugging failing tests, backend MSTest tests, frontend Vitest tests, verifying test results, checking coverage reports."
---

# Unit Testing Guidelines

## Overview
This boilerplate uses **MSTest** for backend (.NET) tests and **Vitest** for frontend (Angular) tests. Always run tests locally before committing, and ensure coverage meets team standards.

---

## When to Run Tests

### Triggers
- After implementing or modifying features
- Before pushing to remote branch
- After fixing bugs
- When refactoring existing code
- During code reviews (verify CI parity locally)

### Prerequisites (All Tests)
- Docker Desktop running
- `.runsettings` file in workspace root

### Prerequisites (Backend Only)
- .NET 9 SDK installed
- Infrastructure services running

### Prerequisites (Frontend Only)
- Node.js 24+ and npm installed

---

## Test Execution Procedures

### Backend Tests (.NET / MSTest)

**1. Start infrastructure services** (required once per session)
```bash
docker compose -f .github/docker-compose/localhost-infra.yml -p datntdev_microservices_infra up -d
```
Wait until all containers are healthy before proceeding.

**2. Build the solution**
```bash
dotnet restore
dotnet build datntdev.Microservice.slnf
```

**3. Clean old test results**
```bash
rm -rf tests/TestResults/*
```

**4. Run database migrations**
```bash
cd srcs/infra/Migrator
dotnet run --no-build
cd ../../..
```

**5. Execute all backend tests**
```bash
dotnet test --no-build --settings .runsettings
```

**6. Generate coverage report** (optional but recommended)
```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator \
  -reports:tests/**/coverage.cobertura.xml \
  -targetdir:tests/TestResults \
  -reporttypes:TextSummary \
  -assemblyfilters:"-*.Tests*"
cat tests/TestResults/Summary.txt
```

### Frontend Tests (Angular / Vitest)

**1. Install dependencies** (required once per environment)
```bash
cd srcs/apps/Angular
npm install
```

**2. Clean old test results**
```bash
rm -rf coverage/
```

**3. Run all tests**
```bash
npm run test:ci
```

### Test Results
- Backend results: `tests/TestResults/`
- Backend coverage: `tests/TestResults/Summary.txt`
- Frontend results: `srcs/apps/Angular/coverage/` (if configured)
- Report failures by **test name**, **error message**, and **file location**

---

## Test Design & Best Practices

### Naming Conventions
- **Test methods**: `MethodUnderTest_GivenCondition_ExpectedBehavior`  
  Example: `CreateUser_GivenValidRequest_ReturnsSuccessResponse`
- **Test classes**: `<ServiceName>Tests` or `<ServiceName><Feature>Tests`
- **Test files**: `*.Tests.cs` (backend) or `*.spec.ts` (frontend)

### Test Structure (Arrange-Act-Assert)
```csharp
[TestMethod]
public void CreateTenant_GivenValidRequest_SuccessfullyCreatesAndReturnsTenantDto()
{
    // Arrange
    var request = new CreateTenantRequest { Name = "Test Tenant" };
    var mockRepository = new Mock<ITenantRepository>();
    var service = new TenantService(mockRepository.Object);

    // Act
    var result = service.Create(request);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual("Test Tenant", result.Name);
    mockRepository.Verify(r => r.SaveAsync(It.IsAny<Tenant>()), Times.Once);
}
```

### Multi-Tenancy in Tests
Every test must establish tenant context when testing domain logic:
```csharp
var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
var tenant = new Tenant { Id = tenantId, Name = "Test Tenant" };
// Use tenantId to seed or mock repository
```

Use base test classes for setup:
- `MicroserviceBaseTest` — Common utilities
- `MicroserviceSrvAdminBaseTest` — Admin service setup
- `MicroserviceSrvIdentityBaseTest` — Identity service setup
- `MicroserviceSrvNotifyBaseTest` — Notify service setup
- `MicroserviceSrvPaymentBaseTest` — Payment service setup

### Test Isolation
- **Mock external dependencies** (databases, HTTP clients, message brokers)
- **Do not make real database calls** in unit tests
- **Use in-memory databases or mocks** for repository tests
- **Avoid shared state** between test methods

### Coverage Expectations
- **Minimum target**: 70% overall code coverage
- **Critical paths**: 90%+ coverage for domain logic, authorization, payment flows
- **Controllers**: 50%+ (focus on happy path, error cases)
- **Use coverage reports** to identify untested code

### Assertions & Validation
- **Assert on observable behavior**, not implementation details
- **Avoid brittle mocks** that enforce internal structure
- **Test edge cases**: null, empty, boundary values, exceptions
- **Verify side effects** (e.g., repository calls, event publishing) using mocks

### Code Placement

| Location | Purpose |
|----------|---------|
| `tests/Srv.Admin/` | Admin service unit & integration tests |
| `tests/Srv.Identity/` | Identity service unit & integration tests |
| `tests/Srv.Notify/` | Notify service unit & integration tests |
| `tests/Srv.Payment/` | Payment service unit & integration tests |
| `tests/Common/` | Shared test utilities and base classes |
| `srcs/apps/Angular/**/*.spec.ts` | Angular component & service tests |

---

## Interpreting Results

### Success Indicators
- ✅ All tests pass (green)
- ✅ Code coverage meets or exceeds threshold (70%+)
- ✅ No warnings in test output
- ✅ Execution time < 2 minutes (backend); < 30 seconds (frontend)

### Debugging Failures
1. **Read the test name and error message** — does it clearly describe the failure?
2. **Check the assertion line** — which condition failed?
3. **Examine mock setup** — did mocks return expected values?
4. **Verify test data** — is tenant context or seed data correct?
5. **Run single test in isolation**: 
   - Backend: `dotnet test --filter "TestClassName.TestMethodName"`
   - Frontend: `npm run test -- --testNamePattern="TestName"`

### Common Issues

| Issue | Solution |
|-------|----------|
| Infrastructure not ready | Ensure Docker containers are healthy: `docker ps -a` |
| Database migration failed | Check migration logs: `cd srcs/infra/Migrator && dotnet run` |
| Tenant context missing | Verify base test class initializes `TenantId` |
| Mock not matching | Ensure mock setup matches actual call signature |
| Coverage below threshold | Add tests for uncovered branches (view `Summary.txt`) |

---

## CI/CD Parity

Local test execution mirrors CI workflows:
- Backend: `.github/workflows/sub_unit_tests.yml`
- Frontend: Same npm commands used in CI

If tests pass locally but fail in CI:
- Verify .NET SDK version matches `global.json`
- Check Node.js version matches `.nvmrc` or `package.json`
- Ensure all infrastructure containers are healthy
- Rebuild and retry: `dotnet clean && dotnet build`
