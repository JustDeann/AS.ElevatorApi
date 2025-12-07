using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Response returned when an elevator is called to a specific floor
public record CallElevatorResponse(
    [property: Display(Name = "Elevator identifier", Description = "Unique identifier of the elevator assigned to the pickup.")]
    Guid ElevatorId,

    [property: Display(Name = "Elevator name", Description = "Human-friendly name of the assigned elevator car.")]
    string ElevatorName,

    [property: Display(Name = "Requested floor", Description = "Pickup floor requested by the caller.")]
    int RequestedFloor,

    [property: Display(Name = "Added to queue", Description = "Indicates whether the pickup floor was newly added to the elevator's stop queue.")]
    bool AddedToQueue,

    [property: Display(Name = "Pending stops", Description = "Current queue of destination floors the elevator will serve, in arrival order.")]
    IReadOnlyCollection<int> PendingStops);
