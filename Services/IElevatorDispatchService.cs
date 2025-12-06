using ElevatorApi.Models;

namespace ElevatorApi.Services;
///Service interface for dispatching and managing elevators
public interface IElevatorDispatchService
{
    CallElevatorResponse RegisterPickup(CallElevatorRequest request);

    AssignDestinationResponse RegisterDestination(AssignDestinationRequest request);

    ElevatorDestinationsResponse GetDestinations(Guid elevatorId);

    NextStopResponse? GetNextStop(Guid elevatorId);

    IReadOnlyCollection<ElevatorSummaryResponse> GetElevators();
}
