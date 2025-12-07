using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ElevatorApi.Options;

namespace ElevatorApi.Models;
//Each request/response type lives in its own file to keep the contract surface discoverable and maintainable.
//Request to call an elevator to a specific floor, optionally specifying the desired travel direction
public class CallElevatorRequest : IValidatableObject
{
    [Required]
    [Display(Name = "Requested floor", Description = "Floor where the elevator should pick up passengers.")]
    [Range(ElevatorDispatchOptions.DefaultMinFloor, ElevatorDispatchOptions.DefaultMaxFloor, ErrorMessage = "Floor must be between the configured minimum and maximum floors.")]
    public int Floor { get; set; }

    [Display(Name = "Requested direction", Description = "Direction to travel after pickup. Accepts 'up' or 'down' (case-insensitive).")]
    public string? Direction { get; set; }

    [JsonIgnore]
    public TravelDirection? RequestedDirection { get; private set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Direction))
        {
            Direction = null;
            RequestedDirection = null;
            yield break;
        }

        var normalizedDirection = Direction.Trim().ToLowerInvariant();
        Direction = normalizedDirection;

        switch (normalizedDirection)
        {
            case "up":
                RequestedDirection = TravelDirection.Up;
                yield break;
            case "down":
                RequestedDirection = TravelDirection.Down;
                yield break;
            default:
                RequestedDirection = null;
                yield return new ValidationResult("Direction must be 'up' or 'down'.", new[] { nameof(Direction) });
                yield break;
        }
    }
}
