using System.Net;
using System.Text;
using OpsCrew.Continuity.Contracts.Documents;
using OpsCrew.Continuity.Core.Modules.Continuity;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Core.Modules.Operations;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Reports.RecoveryActions;

public sealed class RecoveryActionsReportGenerator(
    IFlightReadRepository flightRepository,
    IStandbyAssignmentReadRepository standbyAssignmentRepository,
    IOperationalJournalReadRepository journalRepository)
{
    public async Task<RecoveryActionsReportResponse> GenerateAsync(CancellationToken cancellationToken)
    {
        var flights = await flightRepository.GetFlightsAsync(cancellationToken);
        var standbyAssignments = await standbyAssignmentRepository.GetStandbyAssignmentsAsync(cancellationToken);
        var journal = await journalRepository.GetJournalEntriesAsync(cancellationToken);
        var disruptedFlights = flights
            .Where(flight => flight.Status is "DELAYED" or "CANCELLED" or "DISRUPTED")
            .OrderBy(flight => flight.ScheduledDeparture)
            .ToArray();

        var actions = disruptedFlights
            .Select(flight =>
            {
                var journalEntries = journal
                    .Where(entry => string.Equals(entry.FlightId, flight.FlightId, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(entry => entry.OccurredAt)
                    .ToArray();
                var assignedStandby = standbyAssignments
                    .Where(assignment =>
                        string.Equals(assignment.AssignedFlightId, flight.FlightId, StringComparison.OrdinalIgnoreCase) ||
                        (assignment.AssignedFlightId is null &&
                         assignment.ReadinessStatus is "CONTACTED" or "READY" &&
                         (assignment.BaseIata == flight.OriginIata || assignment.BaseIata == flight.DestinationIata)))
                    .Take(3)
                    .Select(assignment =>
                    {
                        var status = assignment.AssignedFlightId is null ? assignment.ReadinessStatus : "ASSIGNED";
                        return $"{assignment.FullName} ({assignment.EmployeeNumber}) - {status}";
                    })
                    .ToArray();

                return new RecoveryActionItem(
                    flight.Status,
                    new[] { $"{flight.FlightNumber} {flight.OriginIata}-{flight.DestinationIata}" },
                    assignedStandby,
                    GetRecoveryAction(flight.Status),
                    journalEntries.FirstOrDefault()?.OccurredAt ?? flight.ScheduledDeparture,
                    journalEntries.LastOrDefault()?.OccurredAt ?? flight.EstimatedDeparture ?? flight.ScheduledDeparture,
                    flight.DisruptionReason ?? "No operational note provided.");
            })
            .ToArray();

        var warnings = actions.Length == 0
            ? new[] { "No disrupted flights were found for the recovery report." }
            : Array.Empty<string>();
        var rulesApplied = new[]
        {
            "Recovery report includes delayed, cancelled, and disrupted flights.",
            "Assigned standby crew is shown for the impacted flight before base-matched candidates.",
            "Journal timestamps provide first observed and last updated times when available.",
            "Recovery action labels are demo-oriented and not airline-grade decision automation."
        };

        var report = new RecoveryActionsReportResponse(
            $"RECOVERY-ACTIONS-{DateTime.UtcNow:yyyyMMddHHmm}",
            DateTime.UtcNow,
            actions,
            warnings,
            rulesApplied,
            string.Empty);

        return report with { Html = Render(report) };
    }

    private static string GetRecoveryAction(string status)
    {
        return status switch
        {
            "DELAYED" => "Protect rotation and monitor downstream crew duty impact.",
            "CANCELLED" => "Cancel segment, protect passengers and crew, evaluate aircraft swap.",
            "DISRUPTED" => "Coordinate recovery desk, standby crew, and network control review.",
            _ => "Monitor operational state."
        };
    }

    private static string Render(RecoveryActionsReportResponse report)
    {
        var html = new StringBuilder();
        html.AppendLine("<!doctype html><html lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        html.AppendLine("<title>Recovery Actions Report</title>");
        html.AppendLine("<style>");
        html.AppendLine("body{font-family:Arial,sans-serif;margin:0;background:#f4f7fa;color:#182231}.doc{max-width:1120px;margin:0 auto;padding:32px}.sheet{background:#fff;border:1px solid #ccd6e0;padding:28px}.header{display:flex;justify-content:space-between;gap:24px;border-bottom:2px solid #182231;padding-bottom:14px}h1{margin:0;font-size:28px}h2{font-size:15px;margin:24px 0 10px;text-transform:uppercase;letter-spacing:.08em}table{width:100%;border-collapse:collapse}th,td{border-bottom:1px solid #dbe3eb;padding:10px;text-align:left;vertical-align:top}th{font-size:12px;text-transform:uppercase;color:#5a6a7d}.notice{border-left:4px solid #0f766e;background:#effaf7;padding:10px 12px}@media print{body{background:#fff}.doc{padding:0}.sheet{border:0}}");
        html.AppendLine("</style></head><body><main class=\"doc\"><section class=\"sheet\">");
        html.AppendLine("<div class=\"header\"><div><h1>Recovery Actions Report</h1><p>Operational disruption recovery decisions and actions</p></div><div><strong>" + Encode(report.ReportId) + "</strong><br><span>Generated UTC " + report.GeneratedAtUtc.ToString("yyyy-MM-dd HH:mm") + "</span></div></div>");
        html.AppendLine("<h2>Actions</h2><table><thead><tr><th>Disruption</th><th>Impacted flights</th><th>Standby crew</th><th>Recovery action</th><th>Timeline</th><th>Notes</th></tr></thead><tbody>");
        foreach (var action in report.Actions)
        {
            html.AppendLine("<tr><td>" + Encode(action.DisruptionType) + "</td><td>" + Encode(string.Join(", ", action.ImpactedFlights)) + "</td><td>" + Encode(string.Join(", ", action.AssignedStandbyCrew)) + "</td><td>" + Encode(action.RecoveryAction) + "</td><td>" + action.FirstObservedAt.ToString("HH:mm") + " - " + action.LastUpdatedAt.ToString("HH:mm") + "</td><td>" + Encode(action.OperationalNotes) + "</td></tr>");
        }
        html.AppendLine("</tbody></table><h2>Rules Applied</h2><div class=\"notice\"><ul>");
        foreach (var rule in report.RulesApplied)
        {
            html.AppendLine("<li>" + Encode(rule) + "</li>");
        }
        html.AppendLine("</ul></div></section></main></body></html>");
        return html.ToString();
    }

    private static string Encode(string? value)
    {
        return WebUtility.HtmlEncode(value ?? string.Empty);
    }
}
