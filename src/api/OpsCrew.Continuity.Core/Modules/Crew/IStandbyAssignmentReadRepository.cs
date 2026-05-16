using OpsCrew.Continuity.Contracts.Crew;

namespace OpsCrew.Continuity.Core.Modules.Crew;

public interface IStandbyAssignmentReadRepository
{
    Task<IReadOnlyList<StandbyAssignmentResponse>> GetStandbyAssignmentsAsync(CancellationToken cancellationToken);
}
