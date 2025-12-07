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

public class RequestsControllerTests
{
    private readonly Mock<IElevatorDispatchService> _dispatchService = new();

    [Fact]
    public void RequestElevator_ReturnsOkWhenServiceSucceeds()
    {
        var request = new CallElevatorRequest { Floor = 10 };
        var expected = new CallElevatorResponse(Guid.NewGuid(), "CAR-01", 10, true, Array.Empty<int>());
        _dispatchService
            .Setup(service => service.RegisterPickup(It.IsAny<CallElevatorRequest>()))
            .Returns(expected);
        var controller = CreateController();

        var result = controller.RequestElevator(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, okResult.Value);
    }

    [Fact]
    public void RequestElevator_ReturnsValidationProblemWhenModelInvalid()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Floor", "Required");

        var result = controller.RequestElevator(new CallElevatorRequest());

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        var problem = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
        Assert.Contains("Floor", problem.Errors.Keys);
    }

    [Fact]
    public void RequestElevator_ReturnsBadRequestWhenServiceThrowsOutOfRange()
    {
        var request = new CallElevatorRequest { Floor = 100 };
        _dispatchService
            .Setup(service => service.RegisterPickup(It.IsAny<CallElevatorRequest>()))
            .Throws(new ArgumentOutOfRangeException("floor"));
        var controller = CreateController();

        var result = controller.RequestElevator(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var problem = Assert.IsType<ProblemDetails>(badRequest.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
    }

    [Fact]
    public void RequestDestination_ReturnsOkWhenServiceSucceeds()
    {
        var request = new AssignDestinationRequest { ElevatorId = Guid.NewGuid(), Floor = 20 };
        var expected = new AssignDestinationResponse(request.ElevatorId, "CAR-01", 20, true, Array.Empty<int>());
        _dispatchService
            .Setup(service => service.RegisterDestination(It.IsAny<AssignDestinationRequest>()))
            .Returns(expected);
        var controller = CreateController();

        var result = controller.RequestDestination(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, okResult.Value);
    }

    [Fact]
    public void RequestDestination_ReturnsValidationProblemWhenModelInvalid()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("ElevatorId", "Required");

        var result = controller.RequestDestination(new AssignDestinationRequest());

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        var problem = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
        Assert.Contains("ElevatorId", problem.Errors.Keys);
    }

    [Fact]
    public void RequestDestination_ReturnsBadRequestWhenServiceThrowsOutOfRange()
    {
        var request = new AssignDestinationRequest { ElevatorId = Guid.NewGuid(), Floor = 0 };
        _dispatchService
            .Setup(service => service.RegisterDestination(It.IsAny<AssignDestinationRequest>()))
            .Throws(new ArgumentOutOfRangeException("floor"));
        var controller = CreateController();

        var result = controller.RequestDestination(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var problem = Assert.IsType<ProblemDetails>(badRequest.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
    }

    [Fact]
    public void RequestDestination_ReturnsNotFoundWhenServiceThrows()
    {
        var request = new AssignDestinationRequest { ElevatorId = Guid.NewGuid(), Floor = 5 };
        _dispatchService
            .Setup(service => service.RegisterDestination(It.IsAny<AssignDestinationRequest>()))
            .Throws(new KeyNotFoundException("missing"));
        var controller = CreateController();

        var result = controller.RequestDestination(request);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        var problem = Assert.IsType<ProblemDetails>(notFound.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problem.Status);
    }

    private RequestsController CreateController() => new(_dispatchService.Object);
}
