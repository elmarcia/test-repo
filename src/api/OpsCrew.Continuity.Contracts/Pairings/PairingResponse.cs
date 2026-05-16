namespace OpsCrew.Continuity.Contracts.Pairings;

public sealed record PairingResponse(
    string PairingId,
    string PairingCode,
    DateOnly PairingDate,
    IReadOnlyList<string> CrewMemberIds,
    IReadOnlyList<string> FlightIds,
    string Status,
    string LegalityStatus,
    string? LegalityNote);
