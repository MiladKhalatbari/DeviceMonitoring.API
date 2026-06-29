\# Device Monitoring API



A production-style \*\*ASP.NET Core Web API\*\* for managing monitoring devices, recording measurements, and querying operational analytics. Built as a portfolio project to demonstrate backend engineering practices across API design, security, testing, observability, containerization, and CI.



\## Highlights



\* \*\*.NET 10\*\* and ASP.NET Core Web API

\* Layered architecture with separate \*\*API, Services, Data, and Domain\*\* projects

\* JWT authentication with securely hashed passwords using `IPasswordHasher<User>`

\* RESTful endpoints, resource-based routes, validation, and correct HTTP status codes

\* Centralized RFC-style \*\*ProblemDetails\*\* error handling with trace IDs

\* SQL Server persistence through Entity Framework Core migrations and seeded development data

\* Monitoring analytics using efficient LINQ queries and `AsNoTracking()` reads

\* JSON and XML response support with strict content negotiation

\* Structured logging with Serilog

\* Docker Compose startup with SQL Server and automatic EF Core migration execution

\* Unit tests plus integration tests against a real ephemeral SQL Server instance via Testcontainers

\* GitHub Actions CI for restore, build, and test execution



\## Architecture



```text

DeviceMonitoring.API                 HTTP layer, controllers, middleware, startup

DeviceMonitoring.Services            Business rules, DTOs, authentication, analytics

DeviceMonitoring.Data                EF Core context, migrations, repositories

DeviceMonitoring.Domain              Entities and domain models

DeviceMonitoring.UnitTests           Fast isolated unit tests

DeviceMonitoring.IntegrationTests    End-to-end API tests with Testcontainers SQL Server

```



\## Key Technical Capabilities



\### Authentication and security



\* `POST /api/auth/register` creates users with validation and duplicate-username handling.

\* `POST /api/auth/login` validates credentials and returns a JWT access token.

\* Passwords are stored as hashes, never as plain text.

\* Protected API endpoints require a valid bearer token.

\* Invalid credentials return a generic `401 Unauthorized` response.



\### API design and reliability



\* Resource-focused routes such as `/api/devices`, `/api/measurements`, `/api/data/ingest`, and `/api/monitoring/devices/{id}`.

\* DTO validation with data annotations and automatic validation responses from `\[ApiController]`.

\* Consistent HTTP semantics: `201 Created`, `204 No Content`, `400 Bad Request`, `401 Unauthorized`, `404 Not Found`, and `409 Conflict`.

\* Centralized exception handling produces `application/problem+json` responses and includes a request trace ID.

\* Content negotiation supports JSON and XML. Unsupported requested formats return `406 Not Acceptable`.



\### Monitoring and data ingestion



\* Device and measurement CRUD endpoints.

\* Ingestion endpoint that finds or creates a device by name and records the incoming measurement.

\* Analytics for device measurement counts, summaries, averages, minimums, maximums, thresholds, date ranges, latest measurements, and hourly summaries.

\* Read-focused queries use `AsNoTracking()` where appropriate.



\### Quality engineering and delivery



\* Unit tests use xUnit, FakeItEasy, and FluentAssertions.

\* Integration tests run against a real SQL Server container using Testcontainers.

\* The current suite contains \*\*72 passing automated tests\*\*.

\* GitHub Actions CI restores dependencies, builds the solution, and runs the complete test suite.

\* Docker Compose provides a repeatable local environment with the API and SQL Server.



\## Technology Stack



| Area                | Technologies                                                |

| ------------------- | ----------------------------------------------------------- |

| Backend             | .NET 10, ASP.NET Core Web API, C#                           |

| Data                | Entity Framework Core, SQL Server                           |

| Security            | JWT Bearer Authentication, ASP.NET Core PasswordHasher      |

| Validation \& errors | Data Annotations, ProblemDetails, global exception handling |

| Logging             | Serilog                                                     |

| Testing             | xUnit, FakeItEasy, FluentAssertions, Testcontainers.MsSql   |

| Documentation       | Swagger / OpenAPI                                           |

| Delivery            | Docker, Docker Compose, GitHub Actions                      |



\## Run with Docker Compose



```powershell

docker compose up --build

```



\* Swagger UI: `http://localhost:8080/swagger`

\* SQL Server host port: `14333`

\* API port: `8080`



The API automatically applies Entity Framework Core migrations and creates `DeviceMonitoring\_DB`.



```powershell

docker compose down

```



Clean database reset:



```powershell

docker compose down -v

```



\## Run Tests



```powershell

dotnet test

```



Integration tests automatically start and remove an isolated SQL Server container through Testcontainers.



\## Author



\*\*Milad Khalatbari\*\*

Backend / .NET Developer

