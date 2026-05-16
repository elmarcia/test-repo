namespace OpsCrew.Continuity.Contracts.Journal;

public sealed record JournalEntryResponse(
    string JournalEntryId,
    DateTime OccurredAt,
    string Severity,
    string Category,
    string? FlightId,
    string? CrewMemberId,
    string Message,
    string CreatedBy);
