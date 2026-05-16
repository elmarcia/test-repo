namespace OpsCrew.Continuity.Contracts.Flights;

public sealed record DelayFlightRequest(int Minutes, string Reason);
