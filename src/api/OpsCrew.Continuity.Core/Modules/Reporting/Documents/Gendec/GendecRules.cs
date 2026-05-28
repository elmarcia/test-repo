namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.Gendec;

public sealed class GendecRules
{
    public GendecRuleResult Apply(GendecSourceData sourceData)
    {
        var warnings = new List<string>();
        var criticalMessages = new List<string>();
        var rulesApplied = new List<string>
        {
            "Required flight identity fields validated.",
            "Aircraft registration required for operational document preview.",
            "Captain-equivalent crew check applied using CAPTAIN crew role.",
            "Optional missing data reported as generation warnings."
        };

        ValidateRequired(sourceData.Flight.FlightNumber, "flight number", criticalMessages);
        ValidateRequired(sourceData.Flight.AircraftRegistration, "aircraft registration", criticalMessages);
        ValidateRequired(sourceData.Flight.OriginIata, "origin", criticalMessages);
        ValidateRequired(sourceData.Flight.DestinationIata, "destination", criticalMessages);

        if (sourceData.Flight.ScheduledDeparture == default)
        {
            criticalMessages.Add("Scheduled departure is required.");
        }

        var hasCaptain = sourceData.CrewMembers.Any(member => member.CrewRole == "CAPTAIN");
        if (!hasCaptain)
        {
            criticalMessages.Add("Primary crew member or captain-equivalent field is required.");
        }

        if (sourceData.CrewMembers.Count == 0)
        {
            warnings.Add("No crew members were found from pairings for this flight.");
        }

        if (sourceData.Flight.EstimatedDeparture is null || sourceData.Flight.EstimatedArrival is null)
        {
            warnings.Add("Estimated departure or arrival is missing; scheduled times are used for preview context.");
        }

        if (string.IsNullOrWhiteSpace(sourceData.Flight.DisruptionReason)
            && sourceData.Flight.Status is "DELAYED" or "CANCELLED" or "DISRUPTED")
        {
            warnings.Add("Disruption reason is missing for a non-normal flight status.");
        }

        return new GendecRuleResult(criticalMessages, warnings, rulesApplied);
    }

    private static void ValidateRequired(string? value, string fieldName, ICollection<string> messages)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            messages.Add($"{fieldName} is required.");
        }
    }
}

public sealed record GendecRuleResult(
    IReadOnlyList<string> CriticalMessages,
    IReadOnlyList<string> Warnings,
    IReadOnlyList<string> RulesApplied);
