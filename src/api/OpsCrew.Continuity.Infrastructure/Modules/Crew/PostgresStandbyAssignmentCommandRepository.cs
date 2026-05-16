using Npgsql;
using OpsCrew.Continuity.Core.Modules.Crew;

namespace OpsCrew.Continuity.Infrastructure.Modules.Crew;

public sealed class PostgresStandbyAssignmentCommandRepository(NpgsqlDataSource dataSource)
    : IStandbyAssignmentCommandRepository
{
    public async Task<bool> AssignStandbyAsync(
        string standbyAssignmentId,
        string flightId,
        string notes,
        CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        if (!await FlightExistsAsync(connection, transaction, flightId, cancellationToken))
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        const string updateSql = """
            UPDATE operations.standby_assignments
            SET
                readiness_status = 'ASSIGNED',
                notes = @notes
            WHERE standby_assignment_id = @standby_assignment_id
            RETURNING crew_member_id;
            """;

        await using var updateCommand = new NpgsqlCommand(updateSql, connection, transaction);
        updateCommand.Parameters.AddWithValue("standby_assignment_id", standbyAssignmentId);
        updateCommand.Parameters.AddWithValue("notes", notes);

        var crewMemberId = (string?)await updateCommand.ExecuteScalarAsync(cancellationToken);
        if (crewMemberId is null)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        await InsertJournalEntryAsync(
            connection,
            transaction,
            flightId,
            crewMemberId,
            $"Standby assignment {standbyAssignmentId} assigned to flight {flightId}. Notes: {notes}",
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    private static async Task<bool> FlightExistsAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string flightId,
        CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS (SELECT 1 FROM operations.flights WHERE flight_id = @flight_id);";

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("flight_id", flightId);

        return (bool)(await command.ExecuteScalarAsync(cancellationToken) ?? false);
    }

    private static async Task InsertJournalEntryAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string flightId,
        string crewMemberId,
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
                'INFO',
                'Standby',
                @flight_id,
                @crew_member_id,
                @message
            );
            """;

        await using var command = new NpgsqlCommand(insertSql, connection, transaction);
        command.Parameters.AddWithValue("journal_entry_id", $"JRN-{Guid.NewGuid():N}");
        command.Parameters.AddWithValue("occurred_at", DateTime.UtcNow);
        command.Parameters.AddWithValue("flight_id", flightId);
        command.Parameters.AddWithValue("crew_member_id", crewMemberId);
        command.Parameters.AddWithValue("message", message);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
