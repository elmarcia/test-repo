using Npgsql;
using OpsCrew.Continuity.Contracts.Crew;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Infrastructure.Persistence;

namespace OpsCrew.Continuity.Infrastructure.Modules.Crew;

public sealed class PostgresStandbyAssignmentReadRepository(NpgsqlDataSource dataSource)
    : IStandbyAssignmentReadRepository
{
    public async Task<IReadOnlyList<StandbyAssignmentResponse>> GetStandbyAssignmentsAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                standby.standby_assignment_id,
                standby.crew_member_id,
                crew.employee_number,
                crew.full_name,
                standby.base_iata,
                standby.standby_start,
                standby.standby_end,
                standby.readiness_status,
                standby.notes,
                standby.assigned_flight_id,
                flight.flight_number assigned_flight_number,
                flight.origin_iata assigned_origin_iata,
                flight.destination_iata assigned_destination_iata,
                flight.scheduled_departure assigned_scheduled_departure,
                flight.aircraft_registration assigned_aircraft_registration
            FROM operations.standby_assignments standby
            INNER JOIN operations.crew_members crew ON crew.crew_member_id = standby.crew_member_id
            LEFT JOIN operations.flights flight ON flight.flight_id = standby.assigned_flight_id
            ORDER BY standby.standby_start, crew.full_name;
            """;

        await using var command = dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var assignments = new List<StandbyAssignmentResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            assignments.Add(new StandbyAssignmentResponse(
                reader.GetRequiredString("standby_assignment_id"),
                reader.GetRequiredString("crew_member_id"),
                reader.GetRequiredString("employee_number"),
                reader.GetRequiredString("full_name"),
                reader.GetRequiredString("base_iata"),
                reader.GetRequiredDateTime("standby_start"),
                reader.GetRequiredDateTime("standby_end"),
                reader.GetRequiredString("readiness_status"),
                reader.GetNullableString("notes"),
                reader.GetNullableString("assigned_flight_id"),
                reader.GetNullableString("assigned_flight_number"),
                reader.GetNullableString("assigned_origin_iata"),
                reader.GetNullableString("assigned_destination_iata"),
                reader.GetNullableDateTime("assigned_scheduled_departure"),
                reader.GetNullableString("assigned_aircraft_registration")));
        }

        return assignments;
    }
}
