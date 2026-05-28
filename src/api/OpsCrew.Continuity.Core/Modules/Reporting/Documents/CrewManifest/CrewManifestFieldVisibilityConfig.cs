using OpsCrew.Continuity.Contracts.Documents;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.CrewManifest;

public sealed class CrewManifestFieldVisibilityConfig
{
    public bool ShowEmployeeNumber { get; init; } = true;

    public bool ShowBase { get; init; } = true;

    public bool ShowStatus { get; init; } = true;

    public bool ShowLegalityNotes { get; init; } = true;

    public bool ShowWarnings { get; init; } = true;

    public bool ShowRulesApplied { get; init; } = true;

    public CrewManifestFieldVisibility ToContract()
    {
        return new CrewManifestFieldVisibility(
            ShowEmployeeNumber,
            ShowBase,
            ShowStatus,
            ShowLegalityNotes,
            ShowWarnings,
            ShowRulesApplied);
    }
}
