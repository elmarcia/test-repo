using Microsoft.AspNetCore.Mvc;
using OpsCrew.Continuity.Contracts.Journal;
using OpsCrew.Continuity.Core.Modules.Continuity;

namespace OpsCrew.Continuity.Api.Controllers;

[ApiController]
[Route("api/journal")]
public sealed class JournalController(
    OperationalJournalReadService readService,
    OperationalJournalCommandService commandService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<JournalEntryResponse>>> GetJournal(
        CancellationToken cancellationToken)
    {
        var entries = await readService.GetJournalEntriesAsync(cancellationToken);
        return Ok(entries);
    }

    [HttpPost]
    public async Task<IActionResult> AddJournalEntry(
        CreateJournalEntryRequest request,
        CancellationToken cancellationToken)
    {
        var added = await commandService.AddJournalEntryAsync(request, cancellationToken);
        return added ? NoContent() : BadRequest();
    }
}
