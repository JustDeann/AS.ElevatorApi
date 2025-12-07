# AS.ElevatorApi
Senior Consultant – Application Development Practice Test

REST API for simulating a bank of elevators. The service exposes endpoints for requesting pickups, assigning rider destinations, and inspecting each elevator's queue.

## Prerequisites

- .NET 8.0 SDK or newer
- PowerShell 5.1 or Windows Terminal (for the commands below)
- Optional: An HTTP client such as curl, Postman, or Thunder Client

## Getting Started

1. Clone or download the repository.
2. Open a PowerShell prompt in the project root (`AS.ElevatorApi`).
3. Restore dependencies and run the API:
   ```powershell
   dotnet restore
   dotnet run --project ElevatorApi.csproj
   ```
4. The application listens on `http://localhost:8080` (configurable via `Properties/launchSetting.json` or `Program.cs`).

5. For live-reload during development, use `dotnet watch run --project ElevatorApi.csproj`.

## Configuration

- Update `appsettings.json` to adjust elevator dispatch settings (`ElevatorDispatchOptions`).
- `Data/ElevatorSeedData.cs` contains the initial elevator roster and pending stops that are loaded when the service starts.

## API Explorer

- Swagger UI is available at `http://localhost:8080/swagger` while the application is running.


## Available Endpoints

| Method | Path | Description |
| - | - | - |
| GET | `/api/elevators` | List every elevator with its current floor and pending stop count. |
| GET | `/api/elevators/{elevatorId}/destinations` | Retrieve detailed destination queue information for one elevator. |
| POST | `/api/elevators/{elevatorId}/next-stop` | Advance an elevator to its next stop and return the updated state. Returns `204` if no stops remain. |
| POST | `/api/requests/call` | Request an elevator pickup at a floor, optionally indicating travel direction. |
| POST | `/api/requests/destinations` | Assign a rider's destination floor to a specific elevator. |


### Examples

```powershell
# List all elevators
curl http://localhost:8080/api/elevators

# Request a pickup on floor 8 to travel up
curl http://localhost:8080/api/requests/call ^
	-H "Content-Type: application/json" ^
	-d '{"floor":8,"direction": "up"}'

# Inspect the destinations for a specific elevator
curl http://localhost:8080/api/elevators/{elevatorId}/destinations

# Advance an elevator to the next stop
curl -X POST http://localhost:8080/api/elevators/{elevatorId}/next-stop
```

## Testing

Run the project unit tests with:

```powershell
dotnet test
```

## Repository Structure

- `Controllers/` – API controllers that expose elevator operations.
- `Services/` – Core dispatch logic implemented by `ElevatorDispatchService`.
- `Models/` – Request and response DTOs shared by the API.
- `Data/` – Mock data used to run elevator state.
