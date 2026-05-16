using Npgsql;
using OpsCrew.Continuity.Contracts.Journal;
using OpsCrew.Continuity.Core.Modules.Continuity;
using OpsCrew.Continuity.Infrastructure.Persistence;

namespace OpsCrew.Continuity.Infrastructure.Modules.Continuity;

public sealed class PostgresOperationalJournalReadRepository(NpgsqlDataSource dataSource)
    : IOperationalJournalReadRepository
{
    public async Task<IReadOnlyList<JournalEntryResponse>> GetJournalEntriesAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                journal_entry_id,
                occurred_at,
                severity,
                category,
                flight_id,
                crew_member_id,
                message,
                created_by
            FROM operations.operational_journal
            ORDER BY occurred_at DESC, journal_entry_id DESC;
            """;

        await using var command = dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var entries = new List<JournalEntryResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            entries.Add(new JournalEntryResponse(
                reader.GetRequiredString("journal_entry_id"),
                reader.GetRequiredDateTime("occurred_at"),
                reader.GetRequiredString("severity"),
                reader.GetRequiredString("category"),
                reader.GetNullableString("flight_id"),
                reader.GetNullableString("crew_member_id"),
                reader.GetRequiredString("message"),
                reader.GetRequiredString("created_by")));
        }

        return entries;
    }
}
