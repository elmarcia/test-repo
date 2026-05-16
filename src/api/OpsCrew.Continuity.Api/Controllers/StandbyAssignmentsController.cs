using Microsoft.AspNetCore.Mvc;
using OpsCrew.Continuity.Contracts.Crew;
using OpsCrew.Continuity.Core.Modules.Crew;

namespace OpsCrew.Continuity.Api.Controllers;

[ApiController]
[Route("api/standby-assignments")]
public sealed class StandbyAssignmentsController(
    StandbyAssignmentReadService readService,
    StandbyAssignmentCommandService commandService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StandbyAssignmentResponse>>> GetStandbyAssignments(
        CancellationToken cancellationToken)
    {
        var assignments = await readService.GetStandbyAssignmentsAsync(cancellationToken);
        return Ok(assignments);
    }

    [HttpPost("{standbyAssignmentId}/assign")]
    public async Task<IActionResult> AssignStandby(
        string standbyAssignmentId,
        AssignStandbyRequest request,
        CancellationToken cancellationToken)
    {
        var assigned = await commandService.AssignStandbyAsync(
            standbyAssignmentId,
            request.FlightId,
            request.Notes,
            cancellationToken);

        return assigned ? NoContent() : NotFound();
    }
}
