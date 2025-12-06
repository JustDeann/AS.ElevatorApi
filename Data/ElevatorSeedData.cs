namespace ElevatorApi.Data;

internal static class ElevatorSeedData
{
    internal static IReadOnlyCollection<ElevatorSeed> Elevators { get; } = new[]
    {
        new ElevatorSeed(
            Guid.Parse("f5be3f09-12a1-4d53-8f38-1b657871e4ab"),
            "CAR-01",
            1,
            new[] { 8, 15, 22 }),
        new ElevatorSeed(
            Guid.Parse("c0fb4c8f-9af0-4cb0-a6a7-34c90f4ab588"),
            "CAR-02",
            12,
            new[] { 3, 1, 18 }),
        new ElevatorSeed(
            Guid.Parse("5f42997e-8e78-43b9-8f16-9c0b04812177"),
            "CAR-03",
            30,
            new[] { 35, 40 }),
        new ElevatorSeed(
            Guid.Parse("6a2f3f35-5aab-4a85-a725-1ca52ffb62a4"),
            "CAR-04",
            6,
            new[] { 9, 14, 2 })
    };

    internal sealed record ElevatorSeed(Guid Id, string Name, int StartingFloor, IReadOnlyCollection<int> PendingStops);
}
