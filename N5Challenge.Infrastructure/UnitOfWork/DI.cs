using Microsoft.Extensions.DependencyInjection;
using N5Challenge.Infrastructure.UnitOfWork.Interfaces;

namespace N5Challenge.Infrastructure.UnitOfWork;

public static class DI
{
    public static IServiceCollection ConfigureUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, Implementations.UnitOfWork>();
        return services;
    }
}
