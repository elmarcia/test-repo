using OpsCrew.Continuity.Infrastructure;

namespace OpsCrew.Continuity.Api.CompositionRoot;

public static class ModuleRegistration
{
    public static IServiceCollection AddCoreApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        return services;
    }
}
