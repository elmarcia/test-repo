using OpsCrew.Continuity.Contracts.Flights;

namespace OpsCrew.Continuity.Core.Modules.Operations;

public interface IFlightReadRepository
{
    Task<IReadOnlyList<FlightResponse>> GetFlightsAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<FlightResponse>> GetDisruptedFlightsAsync(CancellationToken cancellationToken);
}
