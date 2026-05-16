namespace OpsCrew.Continuity.Contracts.Flights;

public sealed record FlightResponse(
    string FlightId,
    string FlightNumber,
    string OriginIata,
    string DestinationIata,
    DateTime ScheduledDeparture,
    DateTime ScheduledArrival,
    DateTime? EstimatedDeparture,
    DateTime? EstimatedArrival,
    string Status,
    string? DisruptionReason,
    string? AircraftRegistration);
