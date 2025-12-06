using ElevatorApi.Models;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers;
//Controller to manage elevator-related endpoints
[ApiController]
[Route("api/elevators")]
public class ElevatorsController : ControllerBase
{
    private readonly IElevatorDispatchService _dispatchService;

    public ElevatorsController(IElevatorDispatchService dispatchService)
    {
        _dispatchService = dispatchService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ElevatorSummaryResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<ElevatorSummaryResponse>> GetElevators()
    {
        var elevators = _dispatchService.GetElevators();
        return Ok(elevators);
    }

    [HttpGet("{elevatorId:guid}/destinations")]
    [ProducesResponseType(typeof(ElevatorDestinationsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ElevatorDestinationsResponse> GetDestinations(Guid elevatorId)
    {
        try
        {
            var response = _dispatchService.GetDestinations(elevatorId);
            return Ok(response);
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

    [HttpPost("{elevatorId:guid}/next-stop")]
    [ProducesResponseType(typeof(NextStopResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<NextStopResponse> GetNextStop(Guid elevatorId)
    {
        try
        {
            var response = _dispatchService.GetNextStop(elevatorId);
            if (response is null)
            {
                return NoContent();
            }

            return Ok(response);
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
