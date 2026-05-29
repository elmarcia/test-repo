namespace OpsCrew.Continuity.Contracts.Crew;

public sealed record StandbyAssignmentResponse(
    string StandbyAssignmentId,
    string CrewMemberId,
    string EmployeeNumber,
    string FullName,
    string BaseIata,
    DateTime StandbyStart,
    DateTime StandbyEnd,
    string ReadinessStatus,
    string? Notes,
    string? AssignedFlightId,
    string? AssignedFlightNumber,
    string? AssignedOriginIata,
    string? AssignedDestinationIata,
    DateTime? AssignedScheduledDeparture,
    string? AssignedAircraftRegistration);
