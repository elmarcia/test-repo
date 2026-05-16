using Microsoft.AspNetCore.Mvc;
using OpsCrew.Continuity.Contracts.Crew;
using OpsCrew.Continuity.Core.Modules.Crew;

namespace OpsCrew.Continuity.Api.Controllers;

[ApiController]
[Route("api/crew-members")]
public sealed class CrewMembersController(CrewMemberReadService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CrewMemberResponse>>> GetCrewMembers(
        CancellationToken cancellationToken)
    {
        var crewMembers = await service.GetCrewMembersAsync(cancellationToken);
        return Ok(crewMembers);
    }
}
