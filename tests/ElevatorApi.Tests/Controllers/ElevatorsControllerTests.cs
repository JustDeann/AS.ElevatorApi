using System;
using System.Collections.Generic;
using ElevatorApi.Controllers;
using ElevatorApi.Models;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ElevatorApi.Tests.Controllers;

public class ElevatorsControllerTests
{
    private readonly Mock<IElevatorDispatchService> _dispatchService = new();

    [Fact]
    public void GetElevators_ReturnsOkWithPayload()
    {
        var expected = new List<ElevatorSummaryResponse>
        {
            new(
                Guid.NewGuid(),
                "CAR-01",
                5,
                2)
        };

        _dispatchService.Setup(service => service.GetElevators()).Returns(expected);
        var controller = CreateController();

        var result = controller.GetElevators();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, okResult.Value);
    }

    [Fact]
    public void GetDestinations_ReturnsOkWhenElevatorExists()
    {
        var elevatorId = Guid.NewGuid();
        var expected = new ElevatorDestinationsResponse(elevatorId, "CAR-01", 3, new[] { 4, 7 });

        _dispatchService.Setup(service => service.GetDestinations(elevatorId)).Returns(expected);
        var controller = CreateController();

        var result = controller.GetDestinations(elevatorId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, okResult.Value);
    }

    [Fact]
    public void GetDestinations_ReturnsNotFoundWhenServiceThrows()
    {
        var elevatorId = Guid.NewGuid();
        _dispatchService
            .Setup(service => service.GetDestinations(elevatorId))
            .Throws(new KeyNotFoundException("missing"));
        var controller = CreateController();

        var result = controller.GetDestinations(elevatorId);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        var problem = Assert.IsType<ProblemDetails>(notFound.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problem.Status);
    }

    [Fact]
    public void GetNextStop_ReturnsOkWhenNextStopExists()
    {
        var elevatorId = Guid.NewGuid();
        var expected = new NextStopResponse(elevatorId, "CAR-01", 10, new[] { 12 });

        _dispatchService.Setup(service => service.GetNextStop(elevatorId)).Returns(expected);
        var controller = CreateController();

        var result = controller.GetNextStop(elevatorId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, okResult.Value);
    }

    [Fact]
    public void GetNextStop_ReturnsNoContentWhenNoStopsRemain()
    {
        var elevatorId = Guid.NewGuid();
        _dispatchService.Setup(service => service.GetNextStop(elevatorId)).Returns((NextStopResponse?)null);
        var controller = CreateController();

        var result = controller.GetNextStop(elevatorId);

        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public void GetNextStop_ReturnsNotFoundWhenServiceThrows()
    {
        var elevatorId = Guid.NewGuid();
        _dispatchService
            .Setup(service => service.GetNextStop(elevatorId))
            .Throws(new KeyNotFoundException("missing"));
        var controller = CreateController();

        var result = controller.GetNextStop(elevatorId);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        var problem = Assert.IsType<ProblemDetails>(notFound.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problem.Status);
    }

    private ElevatorsController CreateController() => new(_dispatchService.Object);
}
