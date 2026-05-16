namespace OpsCrew.Continuity.Contracts.Journal;

public sealed record CreateJournalEntryRequest(
    string Severity,
    string Category,
    string? FlightId,
    string? CrewMemberId,
    string Message);
