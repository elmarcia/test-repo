using Microsoft.Extensions.DependencyInjection;
using OpsCrew.Continuity.Core.Modules.Continuity;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Core.Modules.Operations;

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

        return services;
    }
}
