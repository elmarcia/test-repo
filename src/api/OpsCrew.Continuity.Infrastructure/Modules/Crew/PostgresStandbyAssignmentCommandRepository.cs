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
                notes = @notes,
                assigned_flight_id = @flight_id
            WHERE standby_assignment_id = @standby_assignment_id
            RETURNING crew_member_id;
            """;

        await using var updateCommand = new NpgsqlCommand(updateSql, connection, transaction);
        updateCommand.Parameters.AddWithValue("standby_assignment_id", standbyAssignmentId);
        updateCommand.Parameters.AddWithValue("notes", notes);
        updateCommand.Parameters.AddWithValue("flight_id", flightId);

        var crewMemberId = (string?)await updateCommand.ExecuteScalarAsync(cancellationToken);
        if (crewMemberId is null)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        await MarkCrewAssignedAsync(connection, transaction, crewMemberId, cancellationToken);
        await AttachCrewToImpactedPairingAsync(connection, transaction, flightId, crewMemberId, cancellationToken);
        var operationalLabel = await GetFlightOperationalLabelAsync(connection, transaction, flightId, cancellationToken);
        var crewLabel = await GetCrewOperationalLabelAsync(connection, transaction, crewMemberId, cancellationToken);

        await InsertJournalEntryAsync(
            connection,
            transaction,
            flightId,
            crewMemberId,
            $"{crewLabel} assigned from standby to {operationalLabel}. Notes: {notes}",
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    private static async Task MarkCrewAssignedAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string crewMemberId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE operations.crew_members
            SET status = 'ASSIGNED'
            WHERE crew_member_id = @crew_member_id;
            """;

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("crew_member_id", crewMemberId);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task AttachCrewToImpactedPairingAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string flightId,
        string crewMemberId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE operations.pairings
            SET crew_member_ids = array_append(crew_member_ids, @crew_member_id)
            WHERE @flight_id = ANY(flight_ids)
              AND NOT @crew_member_id = ANY(crew_member_ids);
            """;

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("flight_id", flightId);
        command.Parameters.AddWithValue("crew_member_id", crewMemberId);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<string> GetFlightOperationalLabelAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string flightId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT flight_number, origin_iata, destination_iata, scheduled_departure, aircraft_registration
            FROM operations.flights
            WHERE flight_id = @flight_id;
            """;

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("flight_id", flightId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return "selected flight";
        }

        var flightNumber = reader.GetString(0);
        var origin = reader.GetString(1);
        var destination = reader.GetString(2);
        var departure = reader.GetDateTime(3).ToString("yyyy-MM-dd HH:mm");
        var aircraft = reader.IsDBNull(4) ? "aircraft TBD" : reader.GetString(4);
        return $"{flightNumber} {origin}->{destination} {aircraft} STD {departure}";
    }

    private static async Task<string> GetCrewOperationalLabelAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string crewMemberId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT full_name, employee_number
            FROM operations.crew_members
            WHERE crew_member_id = @crew_member_id;
            """;

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("crew_member_id", crewMemberId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return "Standby crew";
        }

        return $"{reader.GetString(0)} ({reader.GetString(1)})";
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
