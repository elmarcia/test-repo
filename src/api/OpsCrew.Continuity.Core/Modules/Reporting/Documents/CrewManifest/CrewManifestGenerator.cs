using OpsCrew.Continuity.Contracts.Documents;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.CrewManifest;

public sealed class CrewManifestGenerator(
    CrewManifestDataProvider dataProvider,
    CrewManifestRules rules,
    CrewManifestTemplateRenderer templateRenderer)
{
    private readonly CrewManifestFieldVisibilityConfig fieldVisibility = new();

    public async Task<CrewManifestResponse?> GenerateAsync(
        string flightId,
        CancellationToken cancellationToken)
    {
        var sourceData = await dataProvider.GetSourceDataAsync(flightId, cancellationToken);
        if (sourceData is null)
        {
            return null;
        }

        var ruleResult = rules.Apply(sourceData);
        var flight = sourceData.Flight;
        var document = new CrewManifestResponse(
            flight.FlightId,
            $"CREW-MANIFEST-{flight.FlightNumber}-{DateTime.UtcNow:yyyyMMddHHmm}",
            DateTime.UtcNow,
            new CrewManifestFlight(
                flight.FlightNumber,
                flight.OriginIata,
                flight.DestinationIata,
                flight.ScheduledDeparture,
                flight.ScheduledArrival,
                flight.Status,
                flight.AircraftRegistration),
            sourceData.CrewMembers
                .Select(member => new CrewManifestMember(
                    member.CrewMemberId,
                    member.EmployeeNumber,
                    member.FullName,
                    member.BaseIata,
                    member.CrewRole,
                    member.Status,
                    member.LegalityNote))
                .ToArray(),
            ruleResult.Warnings,
            ruleResult.RulesApplied,
            fieldVisibility.ToContract(),
            string.Empty);

        return document with { Html = templateRenderer.Render(document) };
    }
}
