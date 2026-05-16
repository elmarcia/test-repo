using OpsCrew.Continuity.Contracts.Flights;

namespace OpsCrew.Continuity.Core.Modules.Operations;

public sealed class FlightReadService(IFlightReadRepository repository)
{
    public Task<IReadOnlyList<FlightResponse>> GetFlightsAsync(CancellationToken cancellationToken)
    {
        return repository.GetFlightsAsync(cancellationToken);
    }

    public Task<IReadOnlyList<FlightResponse>> GetDisruptedFlightsAsync(CancellationToken cancellationToken)
    {
        return repository.GetDisruptedFlightsAsync(cancellationToken);
    }
}
