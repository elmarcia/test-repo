using Microsoft.AspNetCore.Mvc;
using OpsCrew.Continuity.Contracts.Documents;
using OpsCrew.Continuity.Core.Modules.Reporting.Documents.CrewManifest;
using OpsCrew.Continuity.Core.Modules.Reporting.Documents.Gendec;
using OpsCrew.Continuity.Core.Modules.Reporting.Reports.RecoveryActions;

namespace OpsCrew.Continuity.Api.Controllers;

[ApiController]
[Route("api/documents")]
public sealed class DocumentsController(
    GendecGenerator gendecGenerator,
    CrewManifestGenerator crewManifestGenerator,
    RecoveryActionsReportGenerator recoveryActionsReportGenerator) : ControllerBase
{
    [HttpGet("gendec/{flightId}")]
    public async Task<ActionResult<GendecDocumentResponse>> GetGendec(
        string flightId,
        CancellationToken cancellationToken)
    {
        var document = await gendecGenerator.GenerateAsync(flightId, cancellationToken);
        return document is null ? NotFound() : Ok(document);
    }

    [HttpGet("gendec/{flightId}/html")]
    public async Task<IActionResult> GetGendecHtml(
        string flightId,
        CancellationToken cancellationToken)
    {
        var document = await gendecGenerator.GenerateAsync(flightId, cancellationToken);
        return document is null
            ? NotFound()
            : Content(document.Html, "text/html");
    }

    [HttpGet("crew-manifest/{flightId}")]
    public async Task<ActionResult<CrewManifestResponse>> GetCrewManifest(
        string flightId,
        CancellationToken cancellationToken)
    {
        var document = await crewManifestGenerator.GenerateAsync(flightId, cancellationToken);
        return document is null ? NotFound() : Ok(document);
    }

    [HttpGet("crew-manifest/{flightId}/html")]
    public async Task<IActionResult> GetCrewManifestHtml(
        string flightId,
        CancellationToken cancellationToken)
    {
        var document = await crewManifestGenerator.GenerateAsync(flightId, cancellationToken);
        return document is null
            ? NotFound()
            : Content(document.Html, "text/html");
    }

    [HttpGet("~/api/reports/recovery-actions")]
    public async Task<ActionResult<RecoveryActionsReportResponse>> GetRecoveryActions(
        CancellationToken cancellationToken)
    {
        var report = await recoveryActionsReportGenerator.GenerateAsync(cancellationToken);
        return Ok(report);
    }

    [HttpGet("~/api/reports/recovery-actions/html")]
    public async Task<IActionResult> GetRecoveryActionsHtml(
        CancellationToken cancellationToken)
    {
        var report = await recoveryActionsReportGenerator.GenerateAsync(cancellationToken);
        return Content(report.Html, "text/html");
    }
}
