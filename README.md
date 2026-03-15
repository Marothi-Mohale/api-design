# Tutoring Platform API

A portfolio-grade ASP.NET Core Web API for a tutoring marketplace. The solution is structured for interview discussion and real-world growth: controller-based endpoints, DTO-driven contracts, JWT authentication, role-based authorization, PostgreSQL via Entity Framework Core, health checks, OpenAPI, structured error handling, and starter tests.

## Solution Structure

- `C:\Users\ASUS\Documents\API DESIGN\TutoringPlatform.Api` - HTTP layer, controllers, middleware, authentication setup, Swagger/OpenAPI, and environment-specific configuration.
- `C:\Users\ASUS\Documents\API DESIGN\TutoringPlatform.Application` - DTOs, service contracts, mapping extensions, pagination models, and application-level exceptions.
- `C:\Users\ASUS\Documents\API DESIGN\TutoringPlatform.Domain` - Core entities and enums for users, tutor profiles, subjects, and tutoring sessions.
- `C:\Users\ASUS\Documents\API DESIGN\TutoringPlatform.Infrastructure` - EF Core `AppDbContext`, PostgreSQL integration, JWT token generation, password hashing, and service implementations.
- `C:\Users\ASUS\Documents\API DESIGN\TutoringPlatform.Tests` - starter xUnit tests for core security and mapping behavior.

## Key Features

- JWT authentication with roles: `Student`, `Tutor`, `Admin`
- RESTful endpoints for auth, subjects, tutors, and tutoring sessions
- DTO-first API surface so EF entities are never returned directly
- Pagination, filtering, and sorting on tutor and session queries
- Global exception middleware returning RFC-style problem details
- Health check endpoint at `/health`
- Swagger/OpenAPI enabled in development
- Environment configuration via `appsettings.json`, `appsettings.Development.json`, and `appsettings.Production.json`

## Planned Database

The project is configured for PostgreSQL through `Npgsql.EntityFrameworkCore.PostgreSQL`. Update the connection string in:

- `C:\Users\ASUS\Documents\API DESIGN\TutoringPlatform.Api\appsettings.Development.json`
- `C:\Users\ASUS\Documents\API DESIGN\TutoringPlatform.Api\appsettings.Production.json`

## Setup

1. Restore packages:

```powershell
dotnet restore .\TutoringPlatform.slnx --configfile .\NuGet.Config
```

2. Build the solution:

```powershell
dotnet build .\TutoringPlatform.slnx --no-restore
```

3. Run the API:

```powershell
dotnet run --project .\TutoringPlatform.Api\TutoringPlatform.Api.csproj
```

4. Open the development docs:

- `http://localhost:5280/docs`
- `http://localhost:5280/openapi/v1.json`

## Local PostgreSQL Workflow

This project includes a repeatable local development database setup:

- [compose.yaml](/C:/Users/ASUS/Documents/API%20DESIGN/compose.yaml) starts PostgreSQL in Docker
- [start-dev-postgres.ps1](/C:/Users/ASUS/Documents/API%20DESIGN/scripts/start-dev-postgres.ps1) starts the database container
- [update-dev-database.ps1](/C:/Users/ASUS/Documents/API%20DESIGN/scripts/update-dev-database.ps1) restores the EF tool and applies migrations
- [seed-dev-data.ps1](/C:/Users/ASUS/Documents/API%20DESIGN/scripts/seed-dev-data.ps1) triggers the development seed endpoint

The initial EF Core migration lives in:

- [20260315133553_InitialCreate.cs](/C:/Users/ASUS/Documents/API%20DESIGN/TutoringPlatform.Infrastructure/Data/Migrations/20260315133553_InitialCreate.cs)

Recommended sequence:

```powershell
docker compose up -d postgres
.\scripts\update-dev-database.ps1
dotnet run --project .\TutoringPlatform.Api\TutoringPlatform.Api.csproj
.\scripts\seed-dev-data.ps1
```

Development-only endpoints are also available while the API is running:

- `POST http://localhost:5280/dev/migrate`
- `POST http://localhost:5280/dev/seed`

## Development Seed Data

In `Development`, the API attempts to create and seed the PostgreSQL database if it is empty. The seed is intentionally lightweight and only runs when there is no existing data.

Demo accounts:

- `admin@tutoringplatform.dev` / `Admin123!`
- `ada@tutoringplatform.dev` / `Tutor123!`
- `grace@tutoringplatform.dev` / `Tutor123!`
- `student@tutoringplatform.dev` / `Student123!`

Seeded data includes:

- core subjects such as Mathematics, Physics, Chemistry, and Computer Science
- two tutor profiles with subject assignments
- one student profile
- one sample upcoming tutoring session

If PostgreSQL is not running, the API will still start in development and log that seed data could not be applied.

## Suggested Next Steps

1. Add EF Core migrations and apply them to PostgreSQL.
2. Add integration tests for authentication and controller workflows.
3. Introduce refresh tokens, email verification, and audit logging if you want to push the project further.
