using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ElevatorApi.Models;
using ElevatorApi.Options;
using ElevatorApi.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace ElevatorApi.Tests.Services;

public class ElevatorDispatchServiceTests
{
    [Fact]
    public void GetElevators_ReturnsConfiguredSummaries()
    {
        var service = CreateService(new ElevatorDispatchOptions { ElevatorCount = 2 });

        var result = service.GetElevators();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, summary => summary.ElevatorName == "CAR-01" && summary.PendingStopsCount == 3);
        Assert.Contains(result, summary => summary.ElevatorName == "CAR-02" && summary.PendingStopsCount == 3);
    }

    [Fact]
    public void RegisterPickup_AddsNewFloorToQueue()
    {
        var service = CreateService(new ElevatorDispatchOptions { ElevatorCount = 2 });
        var request = CreatePickupRequest(25);

        var response = service.RegisterPickup(request);

        Assert.True(response.AddedToQueue);
        Assert.Equal(25, response.PendingStops.Last());

        var destinations = service.GetDestinations(response.ElevatorId);
        Assert.Contains(25, destinations.PendingStops);
    }

    [Fact]
    public void RegisterPickup_ReturnsFalseForDuplicateStop()
    {
        var service = CreateService(new ElevatorDispatchOptions { ElevatorCount = 2 });
        var request = CreatePickupRequest(8, "up");

        var response = service.RegisterPickup(request);

        Assert.False(response.AddedToQueue);
        Assert.Equal(3, response.PendingStops.Count);
    }

    [Fact]
    public void RegisterDestination_ThrowsWhenElevatorMissing()
    {
        var service = CreateService();
        var request = new AssignDestinationRequest
        {
            ElevatorId = Guid.NewGuid(),
            Floor = 10
        };

        Assert.Throws<KeyNotFoundException>(() => service.RegisterDestination(request));
    }

    [Fact]
    public void GetNextStop_ReturnsNullWhenQueueEmpty()
    {
        var service = CreateService(new ElevatorDispatchOptions { ElevatorCount = 1 });
        var elevator = service.GetElevators().Single();

        var first = service.GetNextStop(elevator.ElevatorId);
        Assert.NotNull(first);
        Assert.Equal(8, first!.NextFloor);

        service.GetNextStop(elevator.ElevatorId);
        service.GetNextStop(elevator.ElevatorId);

        var final = service.GetNextStop(elevator.ElevatorId);
        Assert.Null(final);
    }

    private static ElevatorDispatchService CreateService(ElevatorDispatchOptions? options = null)
    {
        var dispatchOptions = options ?? new ElevatorDispatchOptions();
        return new ElevatorDispatchService(Microsoft.Extensions.Options.Options.Create(dispatchOptions));
    }

    private static CallElevatorRequest CreatePickupRequest(int floor, string? direction = null)
    {
        var request = new CallElevatorRequest
        {
            Floor = floor,
            Direction = direction
        };

        _ = request.Validate(new ValidationContext(request)).ToList();
        return request;
    }
}
