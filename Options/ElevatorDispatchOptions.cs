namespace ElevatorApi.Options;
//options for elevator dispatching service
public class ElevatorDispatchOptions
{
    public int ElevatorCount { get; set; } = 4;

    public int MinFloor { get; set; } = 1;

    public int MaxFloor { get; set; } = 50;

    public int DefaultFloor { get; set; } = 1;
}