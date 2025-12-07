using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Response returned when a destination is assigned to an elevator
public record AssignDestinationResponse(
    [property: Display(Name = "Elevator identifier", Description = "Unique identifier of the elevator receiving the destination.")]
    Guid ElevatorId,

    [property: Display(Name = "Elevator name", Description = "Human-friendly name of the elevator car.")]
    string ElevatorName,

    [property: Display(Name = "Destination floor", Description = "Floor assigned to the elevator's queue.")]
    int DestinationFloor,

    [property: Display(Name = "Added to queue", Description = "Indicates whether the destination floor was newly enqueued.")]
    bool AddedToQueue,

    [property: Display(Name = "Pending stops", Description = "Current queue of destination floors scheduled for the elevator.")]
    IReadOnlyCollection<int> PendingStops);
