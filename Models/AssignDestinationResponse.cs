namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Response returned when a destination is assigned to an elevator
public record AssignDestinationResponse(
    Guid ElevatorId,
    string ElevatorName,
    int DestinationFloor,
    bool AddedToQueue,
    IReadOnlyCollection<int> PendingStops);
