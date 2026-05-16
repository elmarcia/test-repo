using OpsCrew.Continuity.Contracts.Journal;

namespace OpsCrew.Continuity.Core.Modules.Continuity;

public sealed class OperationalJournalCommandService(IOperationalJournalCommandRepository repository)
{
    private static readonly HashSet<string> ValidSeverities = ["INFO", "WARNING", "CRITICAL"];

    public Task<bool> AddJournalEntryAsync(
        CreateJournalEntryRequest request,
        CancellationToken cancellationToken)
    {
        var severity = request.Severity.Trim().ToUpperInvariant();

        if (!ValidSeverities.Contains(severity) ||
            string.IsNullOrWhiteSpace(request.Category) ||
            string.IsNullOrWhiteSpace(request.Message))
        {
            return Task.FromResult(false);
        }

        var normalized = request with
        {
            Severity = severity,
            Category = request.Category.Trim(),
            FlightId = string.IsNullOrWhiteSpace(request.FlightId) ? null : request.FlightId.Trim(),
            CrewMemberId = string.IsNullOrWhiteSpace(request.CrewMemberId) ? null : request.CrewMemberId.Trim(),
            Message = request.Message.Trim()
        };

        return AddAsync(normalized, cancellationToken);
    }

    private async Task<bool> AddAsync(
        CreateJournalEntryRequest request,
        CancellationToken cancellationToken)
    {
        await repository.AddJournalEntryAsync(request, cancellationToken);
        return true;
    }
}
