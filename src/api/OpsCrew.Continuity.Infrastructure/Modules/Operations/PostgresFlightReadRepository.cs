using Npgsql;
using OpsCrew.Continuity.Contracts.Flights;
using OpsCrew.Continuity.Core.Modules.Operations;
using OpsCrew.Continuity.Infrastructure.Persistence;

namespace OpsCrew.Continuity.Infrastructure.Modules.Operations;

public sealed class PostgresFlightReadRepository(NpgsqlDataSource dataSource) : IFlightReadRepository
{
    public Task<IReadOnlyList<FlightResponse>> GetFlightsAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                flight_id,
                flight_number,
                origin_iata,
                destination_iata,
                scheduled_departure,
                scheduled_arrival,
                estimated_departure,
                estimated_arrival,
                status,
                disruption_reason,
                aircraft_registration
            FROM operations.flights
            ORDER BY scheduled_departure, flight_number;
            """;

        return QueryFlightsAsync(sql, cancellationToken);
    }

    public Task<IReadOnlyList<FlightResponse>> GetDisruptedFlightsAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                flight_id,
                flight_number,
                origin_iata,
                destination_iata,
                scheduled_departure,
                scheduled_arrival,
                estimated_departure,
                estimated_arrival,
                status,
                disruption_reason,
                aircraft_registration
            FROM operations.flights
            WHERE status IN ('DELAYED', 'CANCELLED', 'DISRUPTED')
            ORDER BY scheduled_departure, flight_number;
            """;

        return QueryFlightsAsync(sql, cancellationToken);
    }

    private async Task<IReadOnlyList<FlightResponse>> QueryFlightsAsync(
        string sql,
        CancellationToken cancellationToken)
    {
        await using var command = dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var flights = new List<FlightResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            flights.Add(new FlightResponse(
                reader.GetRequiredString("flight_id"),
                reader.GetRequiredString("flight_number"),
                reader.GetRequiredString("origin_iata"),
                reader.GetRequiredString("destination_iata"),
                reader.GetRequiredDateTime("scheduled_departure"),
                reader.GetRequiredDateTime("scheduled_arrival"),
                reader.GetNullableDateTime("estimated_departure"),
                reader.GetNullableDateTime("estimated_arrival"),
                reader.GetRequiredString("status"),
                reader.GetNullableString("disruption_reason"),
                reader.GetNullableString("aircraft_registration")));
        }

        return flights;
    }
}
