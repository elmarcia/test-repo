using OpsCrew.Continuity.Contracts.Pairings;

namespace OpsCrew.Continuity.Core.Modules.Crew;

public sealed class PairingReadService(IPairingReadRepository repository)
{
    public Task<IReadOnlyList<PairingResponse>> GetPairingsAsync(CancellationToken cancellationToken)
    {
        return repository.GetPairingsAsync(cancellationToken);
    }
}
