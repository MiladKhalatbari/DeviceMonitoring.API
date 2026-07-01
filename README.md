# Device Monitoring API

A portfolio-focused **ASP.NET Core Web API** for managing monitoring devices, recording measurements, and querying operational analytics.

Built to demonstrate practical backend engineering skills across API design, authentication, data access, testing, containerization, logging, and CI.

**Stack:** .NET 10 · ASP.NET Core · Entity Framework Core · SQL Server · Docker · Testcontainers · GitHub Actions

---

## What This Project Demonstrates

* Clean layered architecture with dedicated API, Service, Data, and Domain projects
* JWT authentication with password hashing through `IPasswordHasher<User>`
* RESTful API design with resource-based routing and meaningful HTTP status codes
* Centralized `ProblemDetails` error handling with request trace IDs
* Entity Framework Core migrations, seeded data, and SQL Server persistence
* Monitoring and analytics queries built with LINQ and optimized read operations
* JSON and XML response support with strict content negotiation
* Structured application and request logging with Serilog
* Docker Compose environment for the API and SQL Server
* Unit tests and real SQL Server integration tests with Testcontainers
* GitHub Actions workflow for restore, build, and automated testing

---

## Architecture

```text
DeviceMonitoring.API
├── Controllers
├── ExceptionHandling
├── Configuration
└── Application startup and middleware

DeviceMonitoring.Services
├── Business logic
├── Authentication
├── DTOs
├── Interfaces
└── Custom exceptions

DeviceMonitoring.Data
├── Entity Framework Core DbContext
├── Repositories
├── Entity configurations
└── Migrations

DeviceMonitoring.Domain
└── Core entities and domain models

DeviceMonitoring.UnitTests
└── Isolated unit tests

DeviceMonitoring.IntegrationTests
└── End-to-end API tests using Testcontainers SQL Server
```

---

## Core Features

### Authentication and Authorization

* Register new users with validation and duplicate username protection
* Authenticate users and return JWT access tokens
* Store passwords as secure hashes rather than plain text
* Protect API endpoints with bearer authentication
* Return generic `401 Unauthorized` responses for invalid credentials

### Device and Measurement Management

* Create, retrieve, update, and delete monitoring devices
* Create, retrieve, update, and delete measurements
* Validate request DTOs automatically through `[ApiController]`
* Return consistent API responses such as:

```text
201 Created
204 No Content
400 Bad Request
401 Unauthorized
404 Not Found
409 Conflict
```

### Data Ingestion

The ingestion endpoint accepts a device name and measurement value.

* Finds an existing device by name
* Creates the device when it does not exist
* Records the incoming measurement
* Returns whether the device was newly created

```text
POST /api/data/ingest
```

### Monitoring Analytics

The API provides monitoring-focused queries for devices and measurements:

* Total measurement count
* Device summary
* Average measurement value
* Minimum and maximum values by date
* Threshold-based measurement counts
* Date-range calculations
* Latest measurements
* Hourly summaries

Read-focused analytics queries use `AsNoTracking()` where appropriate.

---

## Error Handling

Unhandled and expected application exceptions are handled centrally and returned as `application/problem+json`.

Example response:

```json
{
  "title": "Resource not found",
  "status": 404,
  "detail": "Device with identifier '999' was not found.",
  "instance": "/api/devices/999",
  "traceId": "..."
}
```

The API also supports strict content negotiation. When a client requests an unsupported response format, it returns:

```text
406 Not Acceptable
```

---

## Technology Stack

| Area                | Technologies                                           |
| ------------------- | ------------------------------------------------------ |
| Backend             | .NET 10, ASP.NET Core Web API, C#                      |
| Data Access         | Entity Framework Core, SQL Server                      |
| Security            | JWT Bearer Authentication, ASP.NET Core PasswordHasher |
| Validation          | Data Annotations, `[ApiController]`                    |
| Error Handling      | ProblemDetails, global exception handler               |
| Logging             | Serilog                                                |
| Testing             | xUnit, FakeItEasy, FluentAssertions                    |
| Integration Testing | Testcontainers.MsSql                                   |
| API Documentation   | Swagger / OpenAPI                                      |
| Containerization    | Docker, Docker Compose                                 |
| CI                  | GitHub Actions                                         |

---

## Run with Docker Compose

Start the API and SQL Server:

```powershell
docker compose up --build
```

After startup:

| Service    | Address                         |
| ---------- | ------------------------------- |
| Swagger UI | `http://localhost:8080/swagger` |
| API        | `http://localhost:8080`         |
| SQL Server | `localhost,14333`               |

The API automatically applies Entity Framework Core migrations and creates the `DeviceMonitoring_DB` database.

Stop the containers:

```powershell
docker compose down
```

Reset the database completely:

```powershell
docker compose down -v
```

---

## Run Tests

Run the complete test suite:

```powershell
dotnet test
```

The project includes:

* Unit tests for controllers and business logic
* Integration tests that launch a real temporary SQL Server container through Testcontainers
* API-level tests for authentication, devices, measurements, data ingestion, and monitoring endpoints

**Current local test result: 72 passing automated tests.**

---

## Explore the API

Use Swagger after starting Docker Compose:

```text
http://localhost:8080/swagger
```

A local seeded development user is available:

```json
{
  "userName": "admin",
  "password": "123456"
}
```

Authenticate through:

```text
POST /api/auth/login
```

Copy the returned access token into Swagger’s **Authorize** dialog, then test protected endpoints such as:

```text
GET /api/devices
GET /api/measurements
GET /api/monitoring/devices/1/summary
```

---

## Author

**Milad Khalatbari**
Backend / .NET Developer
