using Microsoft.AspNetCore.Mvc;
using OpsCrew.Continuity.Contracts.Flights;
using OpsCrew.Continuity.Core.Modules.Operations;

namespace OpsCrew.Continuity.Api.Controllers;

[ApiController]
[Route("api/flights")]
public sealed class FlightsController(
    FlightReadService readService,
    FlightCommandService commandService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FlightResponse>>> GetFlights(
        CancellationToken cancellationToken)
    {
        var flights = await readService.GetFlightsAsync(cancellationToken);
        return Ok(flights);
    }

    [HttpGet("disrupted")]
    public async Task<ActionResult<IReadOnlyList<FlightResponse>>> GetDisruptedFlights(
        CancellationToken cancellationToken)
    {
        var flights = await readService.GetDisruptedFlightsAsync(cancellationToken);
        return Ok(flights);
    }

    [HttpPost("{flightId}/delay")]
    public async Task<IActionResult> DelayFlight(
        string flightId,
        DelayFlightRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await commandService.DelayFlightAsync(
            flightId,
            request.Minutes,
            request.Reason,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{flightId}/cancel")]
    public async Task<IActionResult> CancelFlight(
        string flightId,
        CancelFlightRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await commandService.CancelFlightAsync(
            flightId,
            request.Reason,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }
}
