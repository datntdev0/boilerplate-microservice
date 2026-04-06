---
name: run-unit-tests
description: 'Run unit tests for the microservice boilerplate. Use when: running tests, executing test suite, checking test results, verifying test coverage, debugging failing tests, backend MSTest .NET tests, frontend Angular Vitest tests, CI parity local test run.'
argument-hint: 'backend | frontend | all (default: all)'
---

# Run Unit Tests

## When to Use
- User asks to run tests, unit tests, or test suite
- Verifying changes before committing
- Diagnosing failing tests after code changes
- Reproducing CI failures locally (mirrors `.github/workflows/sub_unit_tests.yml`)

## Scope

| Argument | Coverage |
|----------|----------|
| `backend` | .NET MSTest projects under `tests/` |
| `frontend` | Angular/Vitest tests under `srcs/apps/Angular/` |
| `all` (default) | Both backend and frontend |

---

## Backend Tests (.NET / MSTest)

### Prerequisites
- Docker Desktop running
- .NET 9 SDK installed

### Procedure

**1. Start infrastructure services**
```bash
docker compose -f .github/docker-compose/localhost-infra.yml -p datntdev_microservices_infra up -d
```
Wait until all containers are healthy before proceeding.

**2. Build the solution**
```bash
dotnet restore
dotnet build datntdev.Microservice.slnf
```

**3. Run database migrations and seed data**
```bash
cd srcs/infra/Migrator
dotnet run --no-build
cd ../../..
```

**4. Run all backend tests**
```bash
dotnet test --no-build --settings .runsettings
```

**5. Generate coverage report** *(optional)*
```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator \
  -reports:tests/**/coverage.cobertura.xml \
  -targetdir:tests/TestResults \
  -reporttypes:TextSummary \
  -assemblyfilters:"-*.Tests*"
cat tests/TestResults/Summary.txt
```

### Test Locations
- `tests/Srv.Admin/` — Admin service tests
- `tests/Srv.Identity/` — Identity service tests
- `tests/Srv.Notify/` — Notify service tests
- `tests/Srv.Payment/` — Payment service tests
- Results saved to `tests/TestResults/`

---

## Frontend Tests (Angular / Vitest)

### Prerequisites
- Node.js 24+ and npm installed

### Procedure

**1. Install dependencies**
```bash
cd srcs/apps/Angular
npm install
```

**2. Run all frontend tests**
```bash
npm run test:ci
```

### Test Locations
- All tests under `srcs/apps/Angular/`
- Results printed to terminal

---

## Surfacing Results
- After running, report the number of passed/failed tests.
- For failures, show the test name, error message, and file location.
- For backend coverage, display the contents of `tests/TestResults/Summary.txt`.
