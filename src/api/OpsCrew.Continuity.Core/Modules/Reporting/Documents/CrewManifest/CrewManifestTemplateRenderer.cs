using System.Net;
using System.Text;
using OpsCrew.Continuity.Contracts.Documents;

namespace OpsCrew.Continuity.Core.Modules.Reporting.Documents.CrewManifest;

public sealed class CrewManifestTemplateRenderer
{
    public string Render(CrewManifestResponse document)
    {
        var html = new StringBuilder();
        html.AppendLine("<!doctype html><html lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        html.AppendLine("<title>Crew Manifest " + Encode(document.Flight.FlightNumber) + "</title>");
        html.AppendLine("<style>");
        html.AppendLine("body{font-family:Arial,sans-serif;margin:0;background:#f4f7fa;color:#182231}.doc{max-width:980px;margin:0 auto;padding:32px}.sheet{background:#fff;border:1px solid #ccd6e0;padding:28px}.header{display:flex;justify-content:space-between;gap:24px;border-bottom:2px solid #182231;padding-bottom:14px}h1{margin:0;font-size:28px}h2{font-size:15px;margin:24px 0 10px;text-transform:uppercase;letter-spacing:.08em}.grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:10px 24px}.field span{display:block;color:#5a6a7d;font-size:12px;text-transform:uppercase}.field strong{font-size:15px}table{width:100%;border-collapse:collapse}th,td{border-bottom:1px solid #dbe3eb;padding:9px;text-align:left}th{font-size:12px;text-transform:uppercase;color:#5a6a7d}.notice{border-left:4px solid #b7791f;background:#fff7e6;padding:10px 12px}.rules{border-left-color:#0f766e;background:#effaf7}@media print{body{background:#fff}.doc{padding:0}.sheet{border:0}}");
        html.AppendLine("</style></head><body><main class=\"doc\"><section class=\"sheet\">");
        html.AppendLine("<div class=\"header\"><div><h1>Crew Manifest</h1><p>Operational continuity demo document</p></div><div><strong>" + Encode(document.DocumentNumber) + "</strong><br><span>Generated UTC " + FormatDate(document.GeneratedAtUtc) + "</span></div></div>");
        html.AppendLine("<h2>Flight</h2><div class=\"grid\">");
        Field(html, "Flight", document.Flight.FlightNumber);
        Field(html, "Route", document.Flight.OriginIata + "-" + document.Flight.DestinationIata);
        Field(html, "Aircraft", document.Flight.AircraftRegistration);
        Field(html, "Scheduled departure", FormatDate(document.Flight.ScheduledDeparture));
        Field(html, "Scheduled arrival", FormatDate(document.Flight.ScheduledArrival));
        Field(html, "Status", document.Flight.Status);
        html.AppendLine("</div>");
        html.AppendLine("<h2>Crew</h2><table><thead><tr><th>Name</th>");
        if (document.FieldVisibility.ShowEmployeeNumber)
        {
            html.AppendLine("<th>Employee</th>");
        }
        html.AppendLine("<th>Role</th>");
        if (document.FieldVisibility.ShowBase)
        {
            html.AppendLine("<th>Base</th>");
        }
        if (document.FieldVisibility.ShowStatus)
        {
            html.AppendLine("<th>Status</th>");
        }
        if (document.FieldVisibility.ShowLegalityNotes)
        {
            html.AppendLine("<th>Legality note</th>");
        }
        html.AppendLine("</tr></thead><tbody>");
        foreach (var member in document.Crew)
        {
            html.AppendLine("<tr><td>" + Encode(member.FullName) + "</td>");
            if (document.FieldVisibility.ShowEmployeeNumber)
            {
                html.AppendLine("<td>" + Encode(member.EmployeeNumber) + "</td>");
            }
            html.AppendLine("<td>" + Encode(member.CrewRole) + "</td>");
            if (document.FieldVisibility.ShowBase)
            {
                html.AppendLine("<td>" + Encode(member.BaseIata) + "</td>");
            }
            if (document.FieldVisibility.ShowStatus)
            {
                html.AppendLine("<td>" + Encode(member.Status) + "</td>");
            }
            if (document.FieldVisibility.ShowLegalityNotes)
            {
                html.AppendLine("<td>" + Encode(member.LegalityNote ?? "None") + "</td>");
            }
            html.AppendLine("</tr>");
        }
        html.AppendLine("</tbody></table>");
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

    private static string FormatDate(DateTime value)
    {
        return value.ToString("yyyy-MM-dd HH:mm");
    }

    private static string Encode(string? value)
    {
        return WebUtility.HtmlEncode(value ?? string.Empty);
    }
}
