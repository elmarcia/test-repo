using OpsCrew.Continuity.Contracts.Crew;

namespace OpsCrew.Continuity.Core.Modules.Crew;

public sealed class CrewMemberReadService(ICrewMemberReadRepository repository)
{
    public Task<IReadOnlyList<CrewMemberResponse>> GetCrewMembersAsync(CancellationToken cancellationToken)
    {
        return repository.GetCrewMembersAsync(cancellationToken);
    }
}
