using Microsoft.Extensions.DependencyInjection;

namespace N5Challenge.Application.DependencyInjection;

public static class DI
{
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DI).Assembly));
        return services;
    }
}
