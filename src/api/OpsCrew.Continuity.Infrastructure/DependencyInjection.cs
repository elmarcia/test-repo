using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using OpsCrew.Continuity.Core.Modules.Continuity;
using OpsCrew.Continuity.Core.Modules.Crew;
using OpsCrew.Continuity.Core.Modules.Operations;
using OpsCrew.Continuity.Infrastructure.Modules.Continuity;
using OpsCrew.Continuity.Infrastructure.Modules.Crew;
using OpsCrew.Continuity.Infrastructure.Modules.Operations;
using OpsCrew.Continuity.Infrastructure.Persistence;

namespace OpsCrew.Continuity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(
            options => options.ConnectionString = configuration.GetConnectionString("Postgres") ?? string.Empty);

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<PostgresOptions>>().Value;
            return new NpgsqlDataSourceBuilder(options.ConnectionString).Build();
        });

        services.AddScoped<IFlightReadRepository, PostgresFlightReadRepository>();
        services.AddScoped<IFlightCommandRepository, PostgresFlightCommandRepository>();
        services.AddScoped<ICrewMemberReadRepository, PostgresCrewMemberReadRepository>();
        services.AddScoped<IStandbyAssignmentReadRepository, PostgresStandbyAssignmentReadRepository>();
        services.AddScoped<IStandbyAssignmentCommandRepository, PostgresStandbyAssignmentCommandRepository>();
        services.AddScoped<IPairingReadRepository, PostgresPairingReadRepository>();
        services.AddScoped<IOperationalJournalReadRepository, PostgresOperationalJournalReadRepository>();
        services.AddScoped<IOperationalJournalCommandRepository, PostgresOperationalJournalCommandRepository>();

        return services;
    }
}
