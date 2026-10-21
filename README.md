# Housing Cost Tracker API

The **Housing Cost Tracker API** is the backend for [Home Cost Tracker](https://github.com/SebastianZins/HousingCostTrackerClient), a household finance application for tracking income, expenses, and transfers between multiple people.

Built with **ASP.NET Core on .NET 10**, the API exposes REST endpoints for managing transactions, people, places, and categories. Data is persisted in **MongoDB**.

## Features

- **Transaction management:** Retrieve, create or update, and delete transactions.
- **People management:** Maintain the people associated with household transactions.
- **Category management:** Organize income and expenses into categories.
- **Place management:** Store merchants, recipients, or other places associated with transactions.
- **Health endpoint:** Check whether the API is responding via `GET /api/health`.
- **MongoDB persistence:** Store application data in a MongoDB database.
- **Layered architecture:** Separate API controllers, services, repositories, data models, and database configuration.
- **OpenAPI document in development:** The API maps an OpenAPI document when running in the Development environment.
- **Private-network-first development:** Authentication is not implemented yet. See the security note below.

## Technology stack

- [ASP.NET Core](https://learn.microsoft.com/aspnet/core/)
- [.NET 10](https://dotnet.microsoft.com/)
- [MongoDB](https://www.mongodb.com/)
- C#
- Newtonsoft.Json
- OpenAPI

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

## Planned work

The following items are planned next:

- **Automated tests:** Add tests for the API's services, repositories, and HTTP endpoints.
- **GitHub Actions:** Add a CI workflow that automatically runs the test suite on pushes and pull requests. The workflow will be added alongside the tests so changes can be validated automatically.

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
