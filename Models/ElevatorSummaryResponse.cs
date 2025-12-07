using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models;
//Summary information for a specific elevator
public record ElevatorSummaryResponse(
    [property: Display(Name = "Elevator identifier", Description = "Unique identifier of the elevator.")]
    Guid ElevatorId,

    [property: Display(Name = "Elevator name", Description = "Human-friendly name used to reference the elevator car.")]
    string ElevatorName,

    [property: Display(Name = "Current floor", Description = "Floor where the elevator car is currently located.")]
    int CurrentFloor,

    [property: Display(Name = "Pending stops count", Description = "Number of floors currently queued for the elevator.")]
    int PendingStopsCount);
