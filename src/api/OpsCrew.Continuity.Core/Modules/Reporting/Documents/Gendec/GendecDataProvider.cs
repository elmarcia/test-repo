using OpsCrew.Continuity.Contracts.Crew;
using OpsCrew.Continuity.Contracts.Flights;
using OpsCrew.Continuity.Contracts.Pairings;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Core.Modules.Operations;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.Gendec;

public sealed class GendecDataProvider(
    IFlightReadRepository flightRepository,
    IPairingReadRepository pairingRepository,
    ICrewMemberReadRepository crewMemberRepository)
{
    public async Task<GendecSourceData?> GetSourceDataAsync(
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
        var matchingCrew = crewMembers
            .Where(crewMember => crewIds.Contains(crewMember.CrewMemberId))
            .OrderBy(crewMember => GetCrewSortOrder(crewMember.CrewRole))
            .ThenBy(crewMember => crewMember.FullName)
            .ToArray();

        return new GendecSourceData(flight, matchingPairings, matchingCrew);
    }

    private static int GetCrewSortOrder(string crewRole)
    {
        return crewRole switch
        {
            "CAPTAIN" => 0,
            "FIRST_OFFICER" => 1,
            "PURSER" => 2,
            _ => 3
        };
    }
}

public sealed record GendecSourceData(
    FlightResponse Flight,
    IReadOnlyList<PairingResponse> Pairings,
    IReadOnlyList<CrewMemberResponse> CrewMembers);
