using Npgsql;
using NpgsqlTypes;
using OpsCrew.Continuity.Contracts.Journal;
using OpsCrew.Continuity.Core.Modules.Continuity;

namespace OpsCrew.Continuity.Infrastructure.Modules.Continuity;

public sealed class PostgresOperationalJournalCommandRepository(NpgsqlDataSource dataSource)
    : IOperationalJournalCommandRepository
{
    public async Task AddJournalEntryAsync(
        CreateJournalEntryRequest request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO operations.operational_journal (
                journal_entry_id,
                occurred_at,
                severity,
                category,
                flight_id,
                crew_member_id,
                message
            ) VALUES (
                @journal_entry_id,
                @occurred_at,
                @severity,
                @category,
                @flight_id,
                @crew_member_id,
                @message
            );
            """;

        await using var command = dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("journal_entry_id", $"JRN-{Guid.NewGuid():N}");
        command.Parameters.AddWithValue("occurred_at", DateTime.UtcNow);
        command.Parameters.AddWithValue("severity", request.Severity);
        command.Parameters.AddWithValue("category", request.Category);
        command.Parameters.Add("flight_id", NpgsqlDbType.Text).Value = (object?)request.FlightId ?? DBNull.Value;
        command.Parameters.Add("crew_member_id", NpgsqlDbType.Text).Value = (object?)request.CrewMemberId ?? DBNull.Value;
        command.Parameters.AddWithValue("message", request.Message);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
