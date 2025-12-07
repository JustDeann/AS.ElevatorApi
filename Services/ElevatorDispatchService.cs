using ElevatorApi.Models;
using ElevatorApi.Data;
using ElevatorApi.Options;
using Microsoft.Extensions.Options;

namespace ElevatorApi.Services;
/// Implementation of the IElevatorDispatchService interface for managing elevator dispatching
public class ElevatorDispatchService : IElevatorDispatchService
{
    private readonly ElevatorDispatchOptions _options;
    private readonly Dictionary<Guid, ElevatorState> _elevators;
    private readonly object _sync = new();

    public ElevatorDispatchService(IOptions<ElevatorDispatchOptions> options)
    {
        _options = options?.Value ?? new ElevatorDispatchOptions();
        ValidateOptions();

        _elevators = new Dictionary<Guid, ElevatorState>(_options.ElevatorCount);

         var seededElevators = ElevatorSeedData.Elevators
            .Take(_options.ElevatorCount)
            .ToList();

        foreach (var seed in seededElevators)
        {
            ValidateFloor(seed.StartingFloor);

            var state = new ElevatorState(seed.Id, seed.Name, seed.StartingFloor);
            foreach (var stop in seed.PendingStops)
            {
                ValidateFloor(stop);
                state.EnqueueStop(stop);
            }

            _elevators[state.Id] = state;
        }

        while (_elevators.Count < _options.ElevatorCount)
        {
            var identifier = Guid.NewGuid();
            var name = $"CAR-{_elevators.Count + 1:D2}";
            _elevators[identifier] = new ElevatorState(identifier, name, _options.DefaultFloor);
        }
    }

    public CallElevatorResponse RegisterPickup(CallElevatorRequest request)
    {
        ValidateFloor(request.Floor);

        lock (_sync)
        {
            var elevator = SelectElevator(request.Floor, request.RequestedDirection);
            var added = elevator.EnqueueStop(request.Floor);

            return new CallElevatorResponse(
                elevator.Id,
                elevator.Name,
                request.Floor,
                added,
                elevator.PendingStops);
        }
    }

    public AssignDestinationResponse RegisterDestination(AssignDestinationRequest request)
    {
        ValidateFloor(request.Floor);

        lock (_sync)
        {
            var elevator = GetElevator(request.ElevatorId);
            var added = elevator.EnqueueStop(request.Floor);

            return new AssignDestinationResponse(
                elevator.Id,
                elevator.Name,
                request.Floor,
                added,
                elevator.PendingStops);
        }
    }

    public ElevatorDestinationsResponse GetDestinations(Guid elevatorId)
    {
        lock (_sync)
        {
            var elevator = GetElevator(elevatorId);
            return new ElevatorDestinationsResponse(
                elevator.Id,
                elevator.Name,
                elevator.CurrentFloor,
                elevator.PendingStops);
        }
    }

    public NextStopResponse? GetNextStop(Guid elevatorId)
    {
        lock (_sync)
        {
            var elevator = GetElevator(elevatorId);
            var nextStop = elevator.DequeueNextStop();

            if (!nextStop.HasValue)
            {
                return null;
            }

            return new NextStopResponse(
                elevator.Id,
                elevator.Name,
                nextStop.Value,
                elevator.PendingStops);
        }
    }

    public IReadOnlyCollection<ElevatorSummaryResponse> GetElevators()
    {
        lock (_sync)
        {
            return _elevators.Values
                .Select(state => new ElevatorSummaryResponse(
                    state.Id,
                    state.Name,
                    state.CurrentFloor,
                    state.PendingStops.Count))
                .ToArray();
        }
    }

    private ElevatorState SelectElevator(int requestedFloor, TravelDirection? direction)
    {
        return _elevators.Values
            .OrderBy(state => state.PendingStops.Count)
            .ThenBy(state => CalculateDirectionalBias(state, requestedFloor, direction))
            .ThenBy(state => Math.Abs(state.CurrentFloor - requestedFloor))
            .ThenBy(state => state.Name)
            .First();
    }

    private static int CalculateDirectionalBias(ElevatorState state, int requestedFloor, TravelDirection? direction)
    {
        return direction switch
        {
            TravelDirection.Up when state.CurrentFloor <= requestedFloor => requestedFloor - state.CurrentFloor,
            TravelDirection.Down when state.CurrentFloor >= requestedFloor => state.CurrentFloor - requestedFloor,
            TravelDirection.Up => int.MaxValue / 2,
            TravelDirection.Down => int.MaxValue / 2,
            _ => Math.Abs(state.CurrentFloor - requestedFloor)
        };
    }

    private ElevatorState GetElevator(Guid elevatorId)
    {
        if (!_elevators.TryGetValue(elevatorId, out var elevator))
        {
            throw new KeyNotFoundException($"Elevator '{elevatorId}' was not found.");
        }

        return elevator;
    }

    private void ValidateFloor(int floor)
    {
        if (floor < _options.MinFloor || floor > _options.MaxFloor)
        {
            throw new ArgumentOutOfRangeException(nameof(floor),
                $"Floor must be between {_options.MinFloor} and {_options.MaxFloor}.");
        }
    }

    private void ValidateOptions()
    {
        if (_options.ElevatorCount <= 0)
        {
            throw new InvalidOperationException("ElevatorCount must be greater than zero.");
        }

        if (_options.MaxFloor < _options.MinFloor)
        {
            throw new InvalidOperationException("MaxFloor must be greater than or equal to MinFloor.");
        }

        if (_options.DefaultFloor < _options.MinFloor || _options.DefaultFloor > _options.MaxFloor)
        {
            throw new InvalidOperationException("DefaultFloor must be within the configured floor range.");
        }
    }

    private sealed class ElevatorState
    {
        private readonly Queue<int> _destinations = new();
        private readonly HashSet<int> _destinationLookup = new();

        internal ElevatorState(Guid id, string name, int startingFloor)
        {
            Id = id;
            Name = name;
            CurrentFloor = startingFloor;
        }

        internal Guid Id { get; }

        internal string Name { get; }

        internal int CurrentFloor { get; private set; }

        internal IReadOnlyCollection<int> PendingStops => _destinations.ToArray();

        internal bool EnqueueStop(int floor)
        {
            if (_destinationLookup.Contains(floor))
            {
                return false;
            }

            _destinations.Enqueue(floor);
            _destinationLookup.Add(floor);
            return true;
        }

        internal int? DequeueNextStop()
        {
            if (_destinations.Count == 0)
            {
                return null;
            }

            var next = _destinations.Dequeue();
            _destinationLookup.Remove(next);
            CurrentFloor = next;
            return next;
        }
    }
}
