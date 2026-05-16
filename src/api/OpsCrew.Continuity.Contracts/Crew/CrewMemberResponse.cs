namespace OpsCrew.Continuity.Contracts.Crew;

public sealed record CrewMemberResponse(
    string CrewMemberId,
    string EmployeeNumber,
    string FullName,
    string BaseIata,
    string CrewRole,
    string Status,
    string? LegalityNote);
