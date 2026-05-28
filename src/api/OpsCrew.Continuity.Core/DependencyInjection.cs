using Microsoft.Extensions.DependencyInjection;
using OpsCrew.Continuity.Core.Modules.Continuity;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Core.Modules.Operations;
using OpsCrew.Continuity.Core.Modules.Reporting.Documents.CrewManifest;
using OpsCrew.Continuity.Core.Modules.Reporting.Documents.Gendec;
using OpsCrew.Continuity.Core.Modules.Reporting.Reports.RecoveryActions;

namespace OpsCrew.Continuity.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<FlightReadService>();
        services.AddScoped<FlightCommandService>();
        services.AddScoped<CrewMemberReadService>();
        services.AddScoped<StandbyAssignmentReadService>();
        services.AddScoped<StandbyAssignmentCommandService>();
        services.AddScoped<PairingReadService>();
        services.AddScoped<OperationalJournalReadService>();
        services.AddScoped<OperationalJournalCommandService>();
        services.AddScoped<GendecDataProvider>();
        services.AddScoped<GendecRules>();
        services.AddScoped<GendecTemplateRenderer>();
        services.AddScoped<GendecGenerator>();
        services.AddScoped<CrewManifestDataProvider>();
        services.AddScoped<CrewManifestRules>();
        services.AddScoped<CrewManifestTemplateRenderer>();
        services.AddScoped<CrewManifestGenerator>();
        services.AddScoped<RecoveryActionsReportGenerator>();

        return services;
    }
}
