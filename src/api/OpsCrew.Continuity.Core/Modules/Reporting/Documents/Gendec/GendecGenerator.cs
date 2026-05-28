using OpsCrew.Continuity.Contracts.Documents;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.Gendec;

public sealed class GendecGenerator(
    GendecDataProvider dataProvider,
    GendecRules rules,
    GendecTemplateRenderer templateRenderer)
{
    private readonly GendecFieldVisibilityConfig fieldVisibility = new();

    public async Task<GendecDocumentResponse?> GenerateAsync(
        string flightId,
        CancellationToken cancellationToken)
    {
        var sourceData = await dataProvider.GetSourceDataAsync(flightId, cancellationToken);
        if (sourceData is null)
        {
            return null;
        }

        var ruleResult = rules.Apply(sourceData);
        var warnings = ruleResult.Warnings.Concat(ruleResult.CriticalMessages).ToArray();
        var flight = sourceData.Flight;
        var crew = sourceData.CrewMembers
            .Select(member => new GendecCrewMember(
                member.CrewMemberId,
                member.EmployeeNumber,
                member.FullName,
                member.CrewRole,
                member.Status))
            .ToArray();

        var document = new GendecDocumentResponse(
            flight.FlightId,
            $"GENDEC-{flight.FlightNumber}-{DateTime.UtcNow:yyyyMMddHHmm}",
            DateTime.UtcNow,
            new GendecFlightInformation(
                flight.FlightNumber,
                flight.ScheduledDeparture,
                flight.ScheduledArrival,
                flight.EstimatedDeparture,
                flight.EstimatedArrival,
                flight.Status,
                flight.DisruptionReason),
            new GendecAircraftInformation(flight.AircraftRegistration),
            new GendecRouteInformation(flight.OriginIata, flight.DestinationIata),
            crew,
            "This General Declaration preview is generated for OPS&CREW Continuity PoC demonstration purposes and is not a regulatory filing.",
            warnings,
            ruleResult.RulesApplied,
            fieldVisibility.ToContract(),
            string.Empty);

        return document with { Html = templateRenderer.Render(document) };
    }
}
