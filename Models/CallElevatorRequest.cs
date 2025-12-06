using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Request to call an elevator to a specific floor, optionally specifying the desired travel direction
public class CallElevatorRequest
{
    [Range(int.MinValue, int.MaxValue)]
    public int Floor { get; set; }

    public TravelDirection? Direction { get; set; }
}
