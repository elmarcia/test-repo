namespace OpsCrew.Continuity.Contracts.Documents;

public sealed record CrewManifestResponse(
    string FlightId,
    string DocumentNumber,
    DateTime GeneratedAtUtc,
    CrewManifestFlight Flight,
    IReadOnlyList<CrewManifestMember> Crew,
    IReadOnlyList<string> Warnings,
    IReadOnlyList<string> RulesApplied,
    CrewManifestFieldVisibility FieldVisibility,
    string Html);

public sealed record CrewManifestFlight(
    string FlightNumber,
    string OriginIata,
    string DestinationIata,
    DateTime ScheduledDeparture,
    DateTime ScheduledArrival,
    string Status,
    string? AircraftRegistration);

public sealed record CrewManifestMember(
    string CrewMemberId,
    string EmployeeNumber,
    string FullName,
    string BaseIata,
    string CrewRole,
    string Status,
    string? LegalityNote);

public sealed record CrewManifestFieldVisibility(
    bool ShowEmployeeNumber,
    bool ShowBase,
    bool ShowStatus,
    bool ShowLegalityNotes,
    bool ShowWarnings,
    bool ShowRulesApplied);
