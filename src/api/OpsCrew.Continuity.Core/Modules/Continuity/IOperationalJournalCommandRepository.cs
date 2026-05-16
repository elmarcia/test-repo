using OpsCrew.Continuity.Contracts.Journal;

namespace OpsCrew.Continuity.Core.Modules.Continuity;

public interface IOperationalJournalCommandRepository
{
    Task AddJournalEntryAsync(
        CreateJournalEntryRequest request,
        CancellationToken cancellationToken);
}
