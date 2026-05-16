using OpsCrew.Continuity.Contracts.Pairings;

namespace OpsCrew.Continuity.Core.Modules.Crew;

public interface IPairingReadRepository
{
    Task<IReadOnlyList<PairingResponse>> GetPairingsAsync(CancellationToken cancellationToken);
}
