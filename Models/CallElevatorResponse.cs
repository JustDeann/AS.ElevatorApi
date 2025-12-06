namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Response returned when an elevator is called to a specific floor
public record CallElevatorResponse(
    Guid ElevatorId,
    string ElevatorName,
    int RequestedFloor,
    bool AddedToQueue,
    IReadOnlyCollection<int> PendingStops);
