namespace OpsCrew.Continuity.Core.Modules.Crew;

public sealed class StandbyAssignmentCommandService(IStandbyAssignmentCommandRepository repository)
{
    public Task<bool> AssignStandbyAsync(
        string standbyAssignmentId,
        string flightId,
        string notes,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(flightId))
        {
            return Task.FromResult(false);
        }

        return repository.AssignStandbyAsync(
            standbyAssignmentId,
            flightId.Trim(),
            notes.Trim(),
            cancellationToken);
    }
}
