# Housing Cost Tracker API

The **Housing Cost Tracker API** is the backend for [Home Cost Tracker](https://github.com/SebastianZins/HousingCostTrackerClient), a household finance application for tracking income, expenses, and transfers between multiple people.

Built with **ASP.NET Core on .NET 10**, the API exposes REST endpoints for managing transactions, people, places, and categories. Data is persisted in **MongoDB**.

## Features

- **Transaction management:** Retrieve, create or update, and delete transactions.
- **People management:** Maintain the people associated with household transactions.
- **Category management:** Organize income and expenses into categories.
- **Place management:** Store merchants, recipients, or other places associated with transactions.
- **MongoDB persistence:** Store application data in a MongoDB database.
- **Layered architecture:** Separate API controllers, services, repositories, data models, and database configuration.
- **OpenAPI document in development:** The API maps an OpenAPI document when running in the Development environment.

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

The controllers use the `/api/{resource}` route pattern. Each resource supports retrieving all records, retrieving a single record by ID, upserting a record, and deleting a record.

| Resource | List all | Get one | Create/update | Delete |
| --- | --- | --- | --- | --- |
| Transactions | `GET /api/transaction/all` | `GET /api/transaction?id={guid}` | `POST /api/transaction` | `DELETE /api/transaction?id={guid}` |
| People | `GET /api/person/all` | `GET /api/person?id={guid}` | `POST /api/person` | `DELETE /api/person?id={guid}` |
| Places | `GET /api/place/all` | `GET /api/place?id={guid}` | `POST /api/place` | `DELETE /api/place?id={guid}` |
| Categories | `GET /api/category/all` | `GET /api/category?id={guid}` | `POST /api/category` | `DELETE /api/category?id={guid}` |

Replace `{guid}` with the ID of the record.

### Health check and OpenAPI

The client repository is configured to probe a `/api/health` endpoint before switching to its local fallback mode. **A health controller is not currently present in this repository's controller list**, so make sure the endpoint is implemented or update the client's health-check configuration before relying on this behavior.

The OpenAPI document is mapped in the Development environment using ASP.NET Core's `MapOpenApi()`. It is not mapped by this code when running in other environments.

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

The application uses HTTPS redirection. The local URL and port depend on your ASP.NET Core launch profile and development environment. Check `Properties/launchSettings.json` or the console output after starting the application.

## Connect the client

The companion Angular client is available here:

**[HousingCostTrackerClient](https://github.com/SebastianZins/HousingCostTrackerClient)**

The client's default API base URL is `https://localhost:1101/api`, configured in `src/environments/environment.ts`. Update this value to match the URL where this API is running.

## Notes

- The API uses GUIDs as record identifiers.
- The `POST` endpoints perform an upsert through the service layer.
- The API's CORS policy is not configured explicitly in the current application startup code. If the client and API are served from different origins, configure CORS for the intended client origin.
- Authentication and authorization policies are not configured here; do not expose the API publicly with sensitive data until suitable access controls are in place.
