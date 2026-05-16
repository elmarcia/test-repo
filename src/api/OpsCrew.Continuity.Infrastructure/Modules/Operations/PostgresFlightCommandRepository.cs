using Npgsql;
using NpgsqlTypes;
using OpsCrew.Continuity.Core.Modules.Operations;

namespace OpsCrew.Continuity.Infrastructure.Modules.Operations;

public sealed class PostgresFlightCommandRepository(NpgsqlDataSource dataSource) : IFlightCommandRepository
{
    public async Task<bool> DelayFlightAsync(
        string flightId,
        int minutes,
        string reason,
        CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string updateSql = """
            UPDATE operations.flights
            SET
                status = 'DELAYED',
                disruption_reason = @reason,
                estimated_departure = COALESCE(estimated_departure, scheduled_departure) + (@minutes * INTERVAL '1 minute'),
                estimated_arrival = COALESCE(estimated_arrival, scheduled_arrival) + (@minutes * INTERVAL '1 minute')
            WHERE flight_id = @flight_id;
            """;

        await using var updateCommand = new NpgsqlCommand(updateSql, connection, transaction);
        updateCommand.Parameters.AddWithValue("flight_id", flightId);
        updateCommand.Parameters.AddWithValue("minutes", minutes);
        updateCommand.Parameters.AddWithValue("reason", reason);

        var updatedRows = await updateCommand.ExecuteNonQueryAsync(cancellationToken);
        if (updatedRows == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        await InsertJournalEntryAsync(
            connection,
            transaction,
            "WARNING",
            "Delay",
            flightId,
            null,
            $"Flight {flightId} delayed by {minutes} minutes. Reason: {reason}",
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    public async Task<bool> CancelFlightAsync(
        string flightId,
        string reason,
        CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string updateSql = """
            UPDATE operations.flights
            SET
                status = 'CANCELLED',
                disruption_reason = @reason,
                estimated_departure = NULL,
                estimated_arrival = NULL
            WHERE flight_id = @flight_id;
            """;

        await using var updateCommand = new NpgsqlCommand(updateSql, connection, transaction);
        updateCommand.Parameters.AddWithValue("flight_id", flightId);
        updateCommand.Parameters.AddWithValue("reason", reason);

        var updatedRows = await updateCommand.ExecuteNonQueryAsync(cancellationToken);
        if (updatedRows == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        await InsertJournalEntryAsync(
            connection,
            transaction,
            "CRITICAL",
            "Cancellation",
            flightId,
            null,
            $"Flight {flightId} cancelled. Reason: {reason}",
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    private static async Task InsertJournalEntryAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string severity,
        string category,
        string? flightId,
        string? crewMemberId,
        string message,
        CancellationToken cancellationToken)
    {
        const string insertSql = """
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

        await using var command = new NpgsqlCommand(insertSql, connection, transaction);
        command.Parameters.AddWithValue("journal_entry_id", $"JRN-{Guid.NewGuid():N}");
        command.Parameters.AddWithValue("occurred_at", DateTime.UtcNow);
        command.Parameters.AddWithValue("severity", severity);
        command.Parameters.AddWithValue("category", category);
        command.Parameters.Add("flight_id", NpgsqlDbType.Text).Value = (object?)flightId ?? DBNull.Value;
        command.Parameters.Add("crew_member_id", NpgsqlDbType.Text).Value = (object?)crewMemberId ?? DBNull.Value;
        command.Parameters.AddWithValue("message", message);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
