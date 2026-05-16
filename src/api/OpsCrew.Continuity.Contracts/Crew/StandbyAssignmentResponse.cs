namespace OpsCrew.Continuity.Contracts.Crew;

public sealed record StandbyAssignmentResponse(
    string StandbyAssignmentId,
    string CrewMemberId,
    string FullName,
    string BaseIata,
    DateTime StandbyStart,
    DateTime StandbyEnd,
    string ReadinessStatus,
    string? Notes);
