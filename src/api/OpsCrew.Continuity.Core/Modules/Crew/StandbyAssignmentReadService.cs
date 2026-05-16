using OpsCrew.Continuity.Contracts.Crew;

namespace OpsCrew.Continuity.Core.Modules.Crew;

public sealed class StandbyAssignmentReadService(IStandbyAssignmentReadRepository repository)
{
    public Task<IReadOnlyList<StandbyAssignmentResponse>> GetStandbyAssignmentsAsync(CancellationToken cancellationToken)
    {
        return repository.GetStandbyAssignmentsAsync(cancellationToken);
    }
}
