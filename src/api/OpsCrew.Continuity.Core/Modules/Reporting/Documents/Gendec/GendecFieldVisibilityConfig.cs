using OpsCrew.Continuity.Contracts.Documents;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.Gendec;

public sealed class GendecFieldVisibilityConfig
{
    public bool ShowEstimatedTimes { get; init; } = true;

    public bool ShowDisruptionReason { get; init; } = true;

    public bool ShowCrewStatus { get; init; } = true;

    public bool ShowWarnings { get; init; } = true;

    public bool ShowRulesApplied { get; init; } = true;

    public GendecFieldVisibility ToContract()
    {
        return new GendecFieldVisibility(
            ShowEstimatedTimes,
            ShowDisruptionReason,
            ShowCrewStatus,
            ShowWarnings,
            ShowRulesApplied);
    }
}
