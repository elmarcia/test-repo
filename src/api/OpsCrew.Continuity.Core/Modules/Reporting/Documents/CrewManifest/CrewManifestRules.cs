namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.CrewManifest;

public sealed class CrewManifestRules
{
    public CrewManifestRuleResult Apply(CrewManifestSourceData sourceData)
    {
        var warnings = new List<string>();
        var rulesApplied = new List<string>
        {
            "Flight and route fields validated for manifest context.",
            "Crew list derived from pairings containing the requested flight.",
            "Captain and first officer presence checked for flight deck completeness.",
            "Legality review crew members surfaced as generation warnings."
        };

        if (sourceData.CrewMembers.Count == 0)
        {
            warnings.Add("No crew members were found for this flight.");
        }

        if (!sourceData.CrewMembers.Any(member => member.CrewRole == "CAPTAIN"))
        {
            warnings.Add("No captain is present in the pairing-derived crew list.");
        }

        if (!sourceData.CrewMembers.Any(member => member.CrewRole == "FIRST_OFFICER"))
        {
            warnings.Add("No first officer is present in the pairing-derived crew list.");
        }

        foreach (var member in sourceData.CrewMembers.Where(member => member.Status == "LEGALITY_REVIEW"))
        {
            warnings.Add($"{member.FullName} is marked for legality review.");
        }

        return new CrewManifestRuleResult(warnings, rulesApplied);
    }
}

public sealed record CrewManifestRuleResult(
    IReadOnlyList<string> Warnings,
    IReadOnlyList<string> RulesApplied);
