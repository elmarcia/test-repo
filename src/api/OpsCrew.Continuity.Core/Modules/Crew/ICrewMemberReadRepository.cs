using OpsCrew.Continuity.Contracts.Crew;

namespace OpsCrew.Continuity.Core.Modules.Crew;

public interface ICrewMemberReadRepository
{
    Task<IReadOnlyList<CrewMemberResponse>> GetCrewMembersAsync(CancellationToken cancellationToken);
}
