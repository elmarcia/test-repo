namespace OpsCrew.Continuity.Contracts.Documents;

public sealed record GendecDocumentResponse(
    string FlightId,
    string DocumentNumber,
    DateTime GeneratedAtUtc,
    GendecFlightInformation FlightInformation,
    GendecAircraftInformation Aircraft,
    GendecRouteInformation Route,
    IReadOnlyList<GendecCrewMember> Crew,
    string Declaration,
    IReadOnlyList<string> Warnings,
    IReadOnlyList<string> RulesApplied,
    GendecFieldVisibility FieldVisibility,
    string Html);

public sealed record GendecFlightInformation(
    string? FlightNumber,
    DateTime? ScheduledDeparture,
    DateTime? ScheduledArrival,
    DateTime? EstimatedDeparture,
    DateTime? EstimatedArrival,
    string? Status,
    string? DisruptionReason);

public sealed record GendecAircraftInformation(string? Registration);

public sealed record GendecRouteInformation(string? OriginIata, string? DestinationIata);

public sealed record GendecCrewMember(
    string CrewMemberId,
    string EmployeeNumber,
    string FullName,
    string CrewRole,
    string Status);

public sealed record GendecFieldVisibility(
    bool ShowEstimatedTimes,
    bool ShowDisruptionReason,
    bool ShowCrewStatus,
    bool ShowWarnings,
    bool ShowRulesApplied);
