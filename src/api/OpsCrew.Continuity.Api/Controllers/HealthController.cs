using Microsoft.AspNetCore.Mvc;
using OpsCrew.Continuity.Contracts.Health;

namespace OpsCrew.Continuity.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<HealthStatusResponse> Get()
    {
        return Ok(new HealthStatusResponse("ok", "OPS&CREW Continuity Core API"));
    }
}
