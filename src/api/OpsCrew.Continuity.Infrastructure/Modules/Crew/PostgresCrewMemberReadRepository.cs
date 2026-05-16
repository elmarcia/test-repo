using Npgsql;
using OpsCrew.Continuity.Contracts.Crew;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Infrastructure.Persistence;

namespace OpsCrew.Continuity.Infrastructure.Modules.Crew;

public sealed class PostgresCrewMemberReadRepository(NpgsqlDataSource dataSource) : ICrewMemberReadRepository
{
    public async Task<IReadOnlyList<CrewMemberResponse>> GetCrewMembersAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                crew_member_id,
                employee_number,
                full_name,
                base_iata,
                crew_role,
                status,
                legality_note
            FROM operations.crew_members
            ORDER BY base_iata, crew_role, full_name;
            """;

        await using var command = dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var crewMembers = new List<CrewMemberResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            crewMembers.Add(new CrewMemberResponse(
                reader.GetRequiredString("crew_member_id"),
                reader.GetRequiredString("employee_number"),
                reader.GetRequiredString("full_name"),
                reader.GetRequiredString("base_iata"),
                reader.GetRequiredString("crew_role"),
                reader.GetRequiredString("status"),
                reader.GetNullableString("legality_note")));
        }

        return crewMembers;
    }
}
