namespace ElevatorApi.Options;
//options for elevator dispatching service
public class ElevatorDispatchOptions
{
    public const int DefaultMinFloor = 1;
    public const int DefaultMaxFloor = 50;

    public int ElevatorCount { get; set; } = 4;

    public int MinFloor { get; set; } = DefaultMinFloor;

    public int MaxFloor { get; set; } = DefaultMaxFloor;

    public int DefaultFloor { get; set; } = DefaultMinFloor;
}