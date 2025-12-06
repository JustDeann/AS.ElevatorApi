namespace ElevatorApi.Models;
//Summary information for a specific elevator
public record ElevatorSummaryResponse(
    Guid ElevatorId,
    string ElevatorName,
    int CurrentFloor,
    int PendingStopsCount);
