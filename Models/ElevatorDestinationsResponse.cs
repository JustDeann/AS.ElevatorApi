using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models;
//Response providing the current destinations and pending stops of a specific elevator
public record ElevatorDestinationsResponse(
    [property: Display(Name = "Elevator identifier", Description = "Unique identifier of the elevator.")]
    Guid ElevatorId,

    [property: Display(Name = "Elevator name", Description = "Human-friendly name of the elevator car.")]
    string ElevatorName,

    [property: Display(Name = "Current floor", Description = "Floor where the elevator car is presently located.")]
    int CurrentFloor,

    [property: Display(Name = "Pending stops", Description = "Floors currently queued for the elevator, ordered by upcoming stop.")]
    IReadOnlyCollection<int> PendingStops);
