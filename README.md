# Housing Cost Tracker API

The **Housing Cost Tracker API** is the backend for [Home Cost Tracker](https://github.com/SebastianZins/HousingCostTrackerClient), a household finance application for tracking income, expenses, and transfers between multiple people.

Built with **ASP.NET Core on .NET 10**, the API exposes REST endpoints for managing transactions, people, places, and categories. Data is persisted in **MongoDB**.

**[View the changelog](CHANGELOG.md)** for major project milestones and timestamps.

## Features

- **Transaction management:** Retrieve, create or update, and delete transactions.
- **People management:** Maintain the people associated with household transactions.
- **Category management:** Organize income and expenses into categories.
- **Place management:** Store merchants, recipients, or other places associated with transactions.
- **Health endpoint:** Check whether the API is responding via `GET /api/health`.
- **MongoDB persistence:** Store application data in a MongoDB database.
- **Layered architecture:** Separate API controllers, services, repositories, data models, and database configuration.
- **Automated unit tests:** Test service behavior, DTO/model mapping, and error handling.
- **GitHub Actions CI:** Restore, build, and test the solution automatically on pushes to `main`.
- **OpenAPI document in development:** The API maps an OpenAPI document when running in the Development environment.
- **Private-network-first development:** Authentication is not implemented yet. See the security note below.

## Technology stack

- [ASP.NET Core](https://learn.microsoft.com/aspnet/core/)
- [.NET 10](https://dotnet.microsoft.com/)
- [MongoDB](https://www.mongodb.com/)
- C#
- Newtonsoft.Json
- OpenAPI
- xUnit and Moq for unit testing
- Testcontainers for MongoDB repository integration tests
- GitHub Actions for continuous integration

## Architecture

The solution is organized into separate projects:

| Project | Responsibility |
| --- | --- |
| `HousingCostTrackerAPI` | ASP.NET Core application and HTTP controllers |
| `HousingCostTrackerAPI.Common` | Shared constants, enums, and interfaces |
| `HousingCostTrackerAPI.Models` | Data models and DTOs |
| `HousingCostTrackerAPI.Repositories` | MongoDB-backed data access |
| `HousingCostTrackerAPI.Services` | Application/business operations |
| `HousingCostTrackerAPI.Database` | MongoDB configuration and dependency registration |
| `HousingCostTrackerAPI.UnitTests` | Service unit tests and MongoDB repository integration tests using xUnit, Moq, and Testcontainers |

## API endpoints

The controllers use the `/api/{resource}` route pattern. Each data resource supports retrieving all records, retrieving a single record by ID, upserting a record, and deleting a record.

| Resource | List all | Get one | Create/update | Delete |
| --- | --- | --- | --- | --- |
| Transactions | `GET /api/transaction/all` | `GET /api/transaction?id={guid}` | `POST /api/transaction` | `DELETE /api/transaction?id={guid}` |
| People | `GET /api/person/all` | `GET /api/person?id={guid}` | `POST /api/person` | `DELETE /api/person?id={guid}` |
| Places | `GET /api/place/all` | `GET /api/place?id={guid}` | `POST /api/place` | `DELETE /api/place?id={guid}` |
| Categories | `GET /api/category/all` | `GET /api/category?id={guid}` | `POST /api/category` | `DELETE /api/category?id={guid}` |

Replace `{guid}` with the ID of the record.

### Health check

`GET /api/health` returns HTTP `200 OK` when the API is responding. The Angular client uses this endpoint to determine whether the backend is reachable before choosing between API-backed storage and its Local Storage fallback.

**Scope:** The current health endpoint only confirms that the API responds; it does not check MongoDB connectivity or whether database operations are healthy.

### OpenAPI

The OpenAPI document is mapped through ASP.NET Core's `MapOpenApi()` in the **Development** environment only. It is not mapped by this code when running in other environments.

## Tests

The test project is located at `HousingCostTrackerAPI/HousingCostTrackerAPI.UnitTests`. The test project is located at `HousingCostTrackerAPI/HousingCostTrackerAPI.UnitTests` and contains both service unit tests and repository integration tests.

### Service unit tests

Service tests use **xUnit** and **Moq** to mock repository dependencies. They do not require MongoDB or Docker.

Coverage includes:

- Retrieving all records, including empty results.
- Retrieving records by ID and mapping models to DTOs.
- Handling missing records and repository failures.
- Creating or updating records and verifying model mapping.
- Deleting records and handling errors.

### MongoDB repository integration tests

Repository tests exercise the real MongoDB driver against a temporary MongoDB container managed by **Testcontainers**. The test suites cover transactions, categories, people, and places, including listing records, inserting and updating records through upsert operations, and deleting records. Transaction repository tests additionally cover retrieving a record by ID and returning `null` for an unknown ID.

**Prerequisites:** Docker Desktop (or another working Docker runtime) must be running. The first run may download the MongoDB image. The tests create an isolated database per test instance and clean the relevant collections before test cases; they do not require a separately installed MongoDB server.

Run all tests from the repository root:

```bash
dotnet restore HousingCostTrackerAPI/HousingCostTrackerAPI.slnx
dotnet test HousingCostTrackerAPI/HousingCostTrackerAPI.slnx --configuration Release
```

Run only service unit tests:

```bash
dotnet test HousingCostTrackerAPI/HousingCostTrackerAPI.UnitTests/HousingCostTrackerAPI.UnitTests.csproj --filter "FullyQualifiedName~UnitTests"
```

Run only repository integration tests (requires Docker):

```bash
dotnet test HousingCostTrackerAPI/HousingCostTrackerAPI.UnitTests/HousingCostTrackerAPI.UnitTests.csproj --filter "FullyQualifiedName~RepositoryTests"
```

## Continuous integration

The [GitHub Actions workflow](.github/workflows/main.yml) runs on every push to the `main` branch. It performs these steps in order:

1. Checks out the repository.
2. Sets up the .NET 10 SDK.
3. Restores solution dependencies.
4. Builds the solution in Release configuration.
5. Runs all tests.

GitHub Actions skips subsequent steps when a previous step fails. A failed restore, build, or test command causes the workflow run to fail. The workflow currently runs on pushes to `main`; it is not configured to run on pull requests.

## Security note

> **Internal-use tool:** This API is currently intended for use on a private network. Authentication and authorization are not implemented yet; the specific authentication design and implementation have not been selected.
>
> Do not expose this API directly to the public internet or use it with sensitive data until appropriate authentication, authorization, and deployment security controls are in place.

## Prerequisites

- .NET 10 SDK
- A running MongoDB instance
- A MongoDB connection string and database name

## Configuration

MongoDB configuration is read from the `MongoDb` section. The application requires both `ConnectionString` and `DatabaseName`; it throws an exception during startup if either is missing.

For local development, use **User Secrets** or environment variables rather than committing connection strings or credentials.

Example environment variables:

```text
MongoDb__ConnectionString=mongodb://localhost:27017
MongoDb__DatabaseName=HousingCostTracker
```

The exact database name is configurable. Set the values to match your MongoDB deployment.

## Run locally

Clone the repository:

```bash
git clone https://github.com/SebastianZins/HousingCostTrackerAPI.git
cd HousingCostTrackerAPI/HousingCostTrackerAPI
```

Configure the MongoDB settings, then run the API:

```bash
dotnet restore
dotnet run --project HousingCostTrackerAPI.csproj
```

The application is configured to listen on `https://localhost:1101` and uses HTTPS redirection.

## Connect the client

The companion Angular client is available here:

**[HousingCostTrackerClient](https://github.com/SebastianZins/HousingCostTrackerClient)**

The client's default API base URL is `https://localhost:1101/api`, configured in `src/environments/environment.ts`. Update this value to match the URL where this API is running.

The API configures a CORS policy named `AngularClient` that allows requests from `http://localhost:4200`. If your client is served from a different origin, update the allowed origin in the API's startup configuration.

## Notes

- The API uses GUIDs as record identifiers.
- The `POST` endpoints perform an upsert through the service layer.
- Authentication and authorization are not implemented yet. This project is intended for internal use on a private network only; see the [Security note](#security-note).

## Documentation

This README was created with the assistance of AI.
