namespace ElevatorApi.Models;
// Next stop information for a specific elevator
public record NextStopResponse(
    Guid ElevatorId,
    string ElevatorName,
    int NextFloor,
    IReadOnlyCollection<int> RemainingStops);