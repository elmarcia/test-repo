using System.Net;
using System.Text;
using OpsCrew.Continuity.Contracts.Documents;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.Gendec;

public sealed class GendecTemplateRenderer
{
    public string Render(GendecDocumentResponse document)
    {
        var html = new StringBuilder();
        html.AppendLine("<!doctype html>");
        html.AppendLine("<html lang=\"en\">");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"utf-8\">");
        html.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        html.AppendLine("<title>GENDEC " + Encode(document.FlightInformation.FlightNumber) + "</title>");
        html.AppendLine("<style>");
        html.AppendLine("body{font-family:Arial,sans-serif;margin:0;background:#f4f7fa;color:#182231}.doc{max-width:960px;margin:0 auto;padding:32px}.sheet{background:#fff;border:1px solid #ccd6e0;padding:28px}.header{display:flex;justify-content:space-between;gap:24px;border-bottom:2px solid #182231;padding-bottom:14px}h1{margin:0;font-size:28px}h2{font-size:15px;margin:24px 0 10px;text-transform:uppercase;letter-spacing:.08em}.grid{display:grid;grid-template-columns:repeat(2,minmax(0,1fr));gap:10px 24px}.field span{display:block;color:#5a6a7d;font-size:12px;text-transform:uppercase}.field strong{font-size:15px}table{width:100%;border-collapse:collapse}th,td{border-bottom:1px solid #dbe3eb;padding:9px;text-align:left}th{font-size:12px;text-transform:uppercase;color:#5a6a7d}.notice{border-left:4px solid #b7791f;background:#fff7e6;padding:10px 12px}.rules{border-left-color:#0f766e;background:#effaf7}@media print{body{background:#fff}.doc{padding:0}.sheet{border:0}}");
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body><main class=\"doc\"><section class=\"sheet\">");
        html.AppendLine("<div class=\"header\"><div><h1>General Declaration</h1><p>Operational continuity demo document</p></div><div><strong>" + Encode(document.DocumentNumber) + "</strong><br><span>Generated UTC " + FormatDate(document.GeneratedAtUtc) + "</span></div></div>");

        html.AppendLine("<h2>Flight Information</h2><div class=\"grid\">");
        Field(html, "Flight", document.FlightInformation.FlightNumber);
        Field(html, "Status", document.FlightInformation.Status);
        Field(html, "Scheduled departure", FormatDate(document.FlightInformation.ScheduledDeparture));
        Field(html, "Scheduled arrival", FormatDate(document.FlightInformation.ScheduledArrival));
        if (document.FieldVisibility.ShowEstimatedTimes)
        {
            Field(html, "Estimated departure", FormatDate(document.FlightInformation.EstimatedDeparture));
            Field(html, "Estimated arrival", FormatDate(document.FlightInformation.EstimatedArrival));
        }
        if (document.FieldVisibility.ShowDisruptionReason)
        {
            Field(html, "Disruption reason", document.FlightInformation.DisruptionReason);
        }
        html.AppendLine("</div>");

        html.AppendLine("<h2>Aircraft</h2><div class=\"grid\">");
        Field(html, "Registration", document.Aircraft.Registration);
        html.AppendLine("</div>");

        html.AppendLine("<h2>Route</h2><div class=\"grid\">");
        Field(html, "Origin", document.Route.OriginIata);
        Field(html, "Destination", document.Route.DestinationIata);
        html.AppendLine("</div>");

        html.AppendLine("<h2>Crew</h2><table><thead><tr><th>Name</th><th>Employee</th><th>Role</th>");
        if (document.FieldVisibility.ShowCrewStatus)
        {
            html.AppendLine("<th>Status</th>");
        }
        html.AppendLine("</tr></thead><tbody>");
        foreach (var crewMember in document.Crew)
        {
            html.AppendLine("<tr><td>" + Encode(crewMember.FullName) + "</td><td>" + Encode(crewMember.EmployeeNumber) + "</td><td>" + Encode(crewMember.CrewRole) + "</td>");
            if (document.FieldVisibility.ShowCrewStatus)
            {
                html.AppendLine("<td>" + Encode(crewMember.Status) + "</td>");
            }
            html.AppendLine("</tr>");
        }
        html.AppendLine("</tbody></table>");

        html.AppendLine("<h2>Declaration</h2><p>" + Encode(document.Declaration) + "</p>");
        if (document.FieldVisibility.ShowWarnings)
        {
            List(html, "Generation warnings", document.Warnings, "notice");
        }
        if (document.FieldVisibility.ShowRulesApplied)
        {
            List(html, "Rules applied", document.RulesApplied, "notice rules");
        }
        html.AppendLine("</section></main></body></html>");
        return html.ToString();
    }

    private static void Field(StringBuilder html, string label, string? value)
    {
        html.AppendLine("<div class=\"field\"><span>" + Encode(label) + "</span><strong>" + Encode(string.IsNullOrWhiteSpace(value) ? "Not provided" : value) + "</strong></div>");
    }

    private static void List(StringBuilder html, string title, IReadOnlyList<string> items, string className)
    {
        html.AppendLine("<h2>" + Encode(title) + "</h2><div class=\"" + className + "\"><ul>");
        if (items.Count == 0)
        {
            html.AppendLine("<li>None</li>");
        }
        foreach (var item in items)
        {
            html.AppendLine("<li>" + Encode(item) + "</li>");
        }
        html.AppendLine("</ul></div>");
    }

    private static string FormatDate(DateTime? value)
    {
        return value?.ToString("yyyy-MM-dd HH:mm") ?? "Not provided";
    }

    private static string Encode(string? value)
    {
        return WebUtility.HtmlEncode(value ?? string.Empty);
    }
}
