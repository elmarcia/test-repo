using OpsCrew.Continuity.Core;
using OpsCrew.Continuity.Infrastructure;

namespace OpsCrew.Continuity.Api.CompositionRoot;

public static class ModuleRegistration
{
    public static IServiceCollection AddCoreApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCoreServices();
        services.AddInfrastructure(configuration);

        return services;
    }
}
