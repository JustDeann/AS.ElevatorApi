namespace ElevatorApi.Models;
//Travel direction of the elevator 
//Used in elevator dispatching logic and status reporting
//Added Stationary to represent elevators that are not moving so callers could explicitly request an elevator that is idle 
public enum TravelDirection
{
    Up = 0,
    Down = 1,
    Stationary = 2
}
