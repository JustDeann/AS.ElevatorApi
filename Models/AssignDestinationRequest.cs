using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Request to assign a destination floor to a specific elevator
public class AssignDestinationRequest
{
    [Required]
    public Guid ElevatorId { get; set; }

    [Range(int.MinValue, int.MaxValue)]
    public int Floor { get; set; }
}
