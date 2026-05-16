namespace OpsCrew.Continuity.Core.Modules.Operations;

public interface IFlightCommandRepository
{
    Task<bool> DelayFlightAsync(
        string flightId,
        int minutes,
        string reason,
        CancellationToken cancellationToken);

    Task<bool> CancelFlightAsync(
        string flightId,
        string reason,
        CancellationToken cancellationToken);
}
