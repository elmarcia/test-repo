using Microsoft.AspNetCore.Mvc;
using OpsCrew.Continuity.Contracts.Pairings;
using OpsCrew.Continuity.Core.Modules.Crew;

namespace OpsCrew.Continuity.Api.Controllers;

[ApiController]
[Route("api/pairings")]
public sealed class PairingsController(PairingReadService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PairingResponse>>> GetPairings(
        CancellationToken cancellationToken)
    {
        var pairings = await service.GetPairingsAsync(cancellationToken);
        return Ok(pairings);
    }
}
