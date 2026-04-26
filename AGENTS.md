# Project Guidelines

## Architecture

This is a microservice architecture with:
- **Frontend**: Angular application (`srcs/apps/Angular/`)
- **Backend Services**: Identity, Admin, Notify, Payment (`srcs/services/`)
- **Infrastructure**: Aspire orchestration, API Gateway (`srcs/infra/`)
- **Shared Libraries**: Common, Application, Communication (`srcs/shared/`)
- **Tests**: Service-specific tests (`tests/`)

## Build and Test

### Infrastructure
- Install docker desktop and ensure it's running
- Run infrastructure: `docker compose -f deploy/dockercompose.local.infra.yml up`

### Frontend (Angular)
- Install: `npm install` (in `srcs/apps/Angular/`)
- Build: `npm run build` (in `srcs/apps/Angular/`)
- Test: `npm run test:ci` (in `srcs/apps/Angular/`)

### Backend (.NET)
- Install: .NET SDK 8.0
- Build: `dotnet build` (from solution root)
- Test: `dotnet test` (from solution root)
- Run: `dotnet run` (in `srcs/infra/Aspire/`)

### E2E Tests
- Install: `npm install` (in `e2e/`)
- Run: `npx playwright test` (in `e2e/`)
