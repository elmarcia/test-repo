using OpsCrew.Continuity.Contracts.Crew;
using OpsCrew.Continuity.Contracts.Flights;
using OpsCrew.Continuity.Contracts.Pairings;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Core.Modules.Operations;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.CrewManifest;

public sealed class CrewManifestDataProvider(
    IFlightReadRepository flightRepository,
    IPairingReadRepository pairingRepository,
    ICrewMemberReadRepository crewMemberRepository)
{
    public async Task<CrewManifestSourceData?> GetSourceDataAsync(
        string flightId,
        CancellationToken cancellationToken)
    {
        var flights = await flightRepository.GetFlightsAsync(cancellationToken);
        var flight = flights.FirstOrDefault(item =>
            string.Equals(item.FlightId, flightId, StringComparison.OrdinalIgnoreCase));

        if (flight is null)
        {
            return null;
        }

        var pairings = await pairingRepository.GetPairingsAsync(cancellationToken);
        var matchingPairings = pairings
            .Where(pairing => pairing.FlightIds.Any(id =>
                string.Equals(id, flight.FlightId, StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        var crewIds = matchingPairings
            .SelectMany(pairing => pairing.CrewMemberIds)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var crewMembers = await crewMemberRepository.GetCrewMembersAsync(cancellationToken);
        var crew = crewMembers
            .Where(member => crewIds.Contains(member.CrewMemberId))
            .OrderBy(member => GetCrewSortOrder(member.CrewRole))
            .ThenBy(member => member.FullName)
            .ToArray();

        return new CrewManifestSourceData(flight, matchingPairings, crew);
    }

    private static int GetCrewSortOrder(string role)
    {
        return role switch
        {
            "CAPTAIN" => 0,
            "FIRST_OFFICER" => 1,
            "PURSER" => 2,
            _ => 3
        };
    }
}

public sealed record CrewManifestSourceData(
    FlightResponse Flight,
    IReadOnlyList<PairingResponse> Pairings,
    IReadOnlyList<CrewMemberResponse> CrewMembers);
