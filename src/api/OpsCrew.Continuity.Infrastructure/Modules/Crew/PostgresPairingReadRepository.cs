using Npgsql;
using OpsCrew.Continuity.Contracts.Pairings;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Infrastructure.Persistence;

namespace OpsCrew.Continuity.Infrastructure.Modules.Crew;

public sealed class PostgresPairingReadRepository(NpgsqlDataSource dataSource) : IPairingReadRepository
{
    public async Task<IReadOnlyList<PairingResponse>> GetPairingsAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                pairing_id,
                pairing_code,
                pairing_date,
                crew_member_ids,
                flight_ids,
                status,
                legality_status,
                legality_note
            FROM operations.pairings
            ORDER BY pairing_date, pairing_code;
            """;

        await using var command = dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var pairings = new List<PairingResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            pairings.Add(new PairingResponse(
                reader.GetRequiredString("pairing_id"),
                reader.GetRequiredString("pairing_code"),
                reader.GetRequiredDateOnly("pairing_date"),
                reader.GetStringArray("crew_member_ids"),
                reader.GetStringArray("flight_ids"),
                reader.GetRequiredString("status"),
                reader.GetRequiredString("legality_status"),
                reader.GetNullableString("legality_note")));
        }

        return pairings;
    }
}
