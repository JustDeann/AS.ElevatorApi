namespace ElevatorApi.Models;
//Response providing the current destinations and pending stops of a specific elevator
public record ElevatorDestinationsResponse(
    Guid ElevatorId,
    string ElevatorName,
    int CurrentFloor,
    IReadOnlyCollection<int> PendingStops);
