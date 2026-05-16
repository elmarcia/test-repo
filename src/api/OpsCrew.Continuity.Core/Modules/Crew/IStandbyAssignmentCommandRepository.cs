namespace OpsCrew.Continuity.Core.Modules.Crew;

public interface IStandbyAssignmentCommandRepository
{
    Task<bool> AssignStandbyAsync(
        string standbyAssignmentId,
        string flightId,
        string notes,
        CancellationToken cancellationToken);
}
