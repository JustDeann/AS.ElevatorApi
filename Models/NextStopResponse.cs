using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models;
// Next stop information for a specific elevator
public record NextStopResponse(
    [property: Display(Name = "Elevator identifier", Description = "Unique identifier of the elevator.")]
    Guid ElevatorId,

    [property: Display(Name = "Elevator name", Description = "Human-friendly name of the elevator car.")]
    string ElevatorName,

    [property: Display(Name = "Next floor", Description = "Floor the elevator will service next.")]
    int NextFloor,

    [property: Display(Name = "Remaining stops", Description = "Floors still queued after the upcoming stop.")]
    IReadOnlyCollection<int> RemainingStops);