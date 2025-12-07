using System.ComponentModel.DataAnnotations;
using ElevatorApi.Options;

namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Request to assign a destination floor to a specific elevator
public class AssignDestinationRequest
{
    [Required]
    [Display(Name = "Elevator identifier", Description = "Unique identifier of the elevator receiving the destination.")]
    public Guid ElevatorId { get; set; }

    [Required]
    [Display(Name = "Destination floor", Description = "Floor to enqueue for the specified elevator.")]
    [Range(ElevatorDispatchOptions.DefaultMinFloor, ElevatorDispatchOptions.DefaultMaxFloor, ErrorMessage = "Floor must be between the configured minimum and maximum floors.")]
    public int Floor { get; set; }
}
