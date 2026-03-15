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

4. Open Swagger in development:

- `https://localhost:<port>/swagger`

## Suggested Next Steps

1. Add EF Core migrations and apply them to PostgreSQL.
2. Add integration tests for authentication and controller workflows.
3. Introduce refresh tokens, email verification, and audit logging if you want to push the project further.
