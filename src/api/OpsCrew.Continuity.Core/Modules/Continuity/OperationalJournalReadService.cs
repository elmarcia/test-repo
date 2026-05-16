using OpsCrew.Continuity.Contracts.Journal;

namespace OpsCrew.Continuity.Core.Modules.Continuity;

public sealed class OperationalJournalReadService(IOperationalJournalReadRepository repository)
{
    public Task<IReadOnlyList<JournalEntryResponse>> GetJournalEntriesAsync(CancellationToken cancellationToken)
    {
        return repository.GetJournalEntriesAsync(cancellationToken);
    }
}
