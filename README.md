# Kongroo

Kongroo is the Phase 1 MVP for FIAP Cloud Games (FCG). The goal of this repository is to provide the foundation for user registration, authentication and authorization, game catalog management, order placement, and synchronization of each player's acquired-game library.

## Current implementation

- Modular monolith organized by business capability.
- ASP.NET Core Minimal API host.
- PostgreSQL persistence with Entity Framework Core migrations.
- JWT authentication with `User` and `Admin` roles.
- OpenAPI exposure with Scalar in the Development environment.
- Structured logging, health checks, domain events, integration events, and outbox-based internal integration.
- Current target framework is `net10.0`.

## Requirements coverage

| Challenge requirement | Status | Evidence in this repository |
| --- | --- | --- |
| User registration with name, email, and password | Implemented | `POST /identity/users` plus domain validation and request validation attributes |
| Email format and strong password validation | Implemented | `CreateUserRequest` enforces email format and password complexity |
| JWT authentication | Implemented | `POST /identity/tokens` issues bearer tokens |
| User and admin authorization levels | Implemented | Authenticated routes plus `AdminOnly` policy for administrative operations |
| Monolith architecture for MVP delivery | Implemented | Single API host with modularized `Identity`, `Catalog`, and `Library` modules |
| EF Core persistence and migrations | Implemented | PostgreSQL + `DbContext` migrations applied on startup in Development |
| Minimal API or MVC | Implemented | Minimal API route groups in the presentation layer |
| Error handling and structured logs | Implemented | Exception handler, Problem Details, status code pages, and Serilog |
| Swagger/OpenAPI documentation | Implemented | OpenAPI + Scalar exposed in Development |
| Unit tests for business rules | Implemented | Unit test project under `tests/Kongroo.CloudGames.UnitTests` |
| TDD or BDD in at least one module | Automated tests are present | Unit and integration tests cover domain and application behavior |
| DDD organization and business rules | Implemented | Domain entities, value objects, domain events, and module boundaries |
| Event Storming documentation | Linked | Miro board: [Event Storming workspace](https://miro.com/app/board/uXjVGvYUERE=/?share_link_id=411419276450) |
| Optional MongoDB, Dapper, GraphQL, Domain Storytelling | Not in current tracked scope | Not required for Phase 1 delivery |

## Project structure

- `src/Kongroo.CloudGames.Api` - API host, authentication pipeline, OpenAPI, health checks, and startup migration application.
- `src/Kongroo.CloudGames.Identity` - user registration, login, user queries, and role changes.
- `src/Kongroo.CloudGames.Catalog` - games, promotions, and purchase orders.
- `src/Kongroo.CloudGames.Library` - acquired game ownership records synchronized after completed orders.
- `src/Kongroo.BuildingBlocks` - shared domain abstractions, event bus contracts, authorization helpers, and outbox processing.
- `tests` - unit and integration test projects for domain, application, infrastructure, and module interaction scenarios.

## Running locally

### Prerequisites

- Docker Desktop or another Docker-compatible engine.
- A `dotnet` SDK that can build and run `net10.0`.
- Optional: local tool restore for `dotnet-ef` and `csharpier`.

### Start infrastructure

```bash
docker compose up -d
```

### Restore local tools

```bash
dotnet tool restore
```

### Run the API

```bash
dotnet run --project src/Kongroo.CloudGames.Api
```

When the API starts `ASPNETCORE_ENVIRONMENT` is set to `Development`. In this environment the application automatically applies the EF Core migrations for the `catalog`, `identity`, and `library` schemas.

### Default local endpoints

| Surface | URL | Notes |
| --- | --- | --- |
| API (HTTPS) | `https://localhost:7082` | Main app URL from the `https` launch profile |
| API (HTTP) | `http://localhost:5282` | Also exposed by the same launch profile |
| Scalar | `https://localhost:7082/scalar` | Development-only API reference UI |
| Health checks | `https://localhost:7082/health` | Aggregated health endpoint |
| pgAdmin | `http://localhost:5050` | Login: `admin@kongroo.dev` / `development` |
| PostgreSQL | `localhost:5432` | Database: `kongroo`, user: `kongroo`, password: `development` |

## Authentication and bootstrap

- Create accounts through `POST /identity/users`.
- Obtain bearer tokens through `POST /identity/tokens`.
- Every new account starts with the `User` role.
- Admin-only endpoints exist for game management and user role changes.

`BootstrapAdmin` is required configuration in every environment. In local development the repository already provides a default value in [appsettings.Development.json](src/Kongroo.CloudGames.Api/appsettings.Development.json).

At startup the application checks the `identity.users` table:

- If there are no users yet, it creates the configured bootstrap account with the `Admin` role.
- If any user already exists, bootstrap is skipped without changing existing accounts.

After the admin account is bootstrapped, create a token with `POST /identity/tokens` so the JWT contains the `Admin` role claim.

## Tests

Run the full automated suite from the repository root:

```bash
dotnet test Kongroo.slnx
```

- Unit tests live in `tests/Kongroo.CloudGames.UnitTests`.
- Integration tests live in `tests/Kongroo.CloudGames.IntegrationTests`.
- Integration tests use Testcontainers with PostgreSQL, so Docker must be available.

## Next steps / Phase gaps

- Add the delivery video link when it is available.
