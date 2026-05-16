namespace OpsCrew.Continuity.Core.Modules.Operations;

public sealed class FlightCommandService(IFlightCommandRepository repository)
{
    public Task<bool> DelayFlightAsync(
        string flightId,
        int minutes,
        string reason,
        CancellationToken cancellationToken)
    {
        if (minutes <= 0 || string.IsNullOrWhiteSpace(reason))
        {
            return Task.FromResult(false);
        }

        return repository.DelayFlightAsync(flightId, minutes, reason.Trim(), cancellationToken);
    }

    public Task<bool> CancelFlightAsync(
        string flightId,
        string reason,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Task.FromResult(false);
        }

        return repository.CancelFlightAsync(flightId, reason.Trim(), cancellationToken);
    }
}
