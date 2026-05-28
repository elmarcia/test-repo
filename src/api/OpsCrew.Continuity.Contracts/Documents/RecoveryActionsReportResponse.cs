namespace OpsCrew.Continuity.Contracts.Documents;

public sealed record RecoveryActionsReportResponse(
    string ReportId,
    DateTime GeneratedAtUtc,
    IReadOnlyList<RecoveryActionItem> Actions,
    IReadOnlyList<string> Warnings,
    IReadOnlyList<string> RulesApplied,
    string Html);

public sealed record RecoveryActionItem(
    string DisruptionType,
    IReadOnlyList<string> ImpactedFlights,
    IReadOnlyList<string> AssignedStandbyCrew,
    string RecoveryAction,
    DateTime FirstObservedAt,
    DateTime LastUpdatedAt,
    string OperationalNotes);
