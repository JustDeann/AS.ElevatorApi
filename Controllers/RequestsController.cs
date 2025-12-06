using ElevatorApi.Models;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers;
///Controller to manage elevator request-related endpoints
[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    private readonly IElevatorDispatchService _dispatchService;

    public RequestsController(IElevatorDispatchService dispatchService)
    {
        _dispatchService = dispatchService;
    }

    [HttpPost("call")]
    [ProducesResponseType(typeof(CallElevatorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CallElevatorResponse> RequestElevator([FromBody] CallElevatorRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var response = _dispatchService.RegisterPickup(request);
            return Ok(response);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Floor out of range",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPost("destinations")]
    [ProducesResponseType(typeof(AssignDestinationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<AssignDestinationResponse> RequestDestination([FromBody] AssignDestinationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var response = _dispatchService.RegisterDestination(request);
            return Ok(response);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Floor out of range",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Elevator not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }
}
