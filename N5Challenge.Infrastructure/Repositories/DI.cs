using Microsoft.Extensions.DependencyInjection;
using N5Challenge.Domain.Entities.Permissions;
using N5Challenge.Infrastructure.Repositories.Implementations;
using N5Challenge.Infrastructure.Repositories.Interfaces;

namespace N5Challenge.Infrastructure.Repositories.DependencyInjection;

public static class DI
{
    public static IServiceCollection ConfigureRepos(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Permission>, PermissionsRepository>();
        services.AddScoped<IRepository<PermissionType>, PermissionTypesRepository>();

        services.AddScoped<IPermissionsRepository>(svc => (PermissionsRepository)svc.GetRequiredService<IRepository<Permission>>());
        services.AddScoped<IPermissionTypesRepository>(svc => (PermissionTypesRepository)svc.GetRequiredService<IRepository<PermissionType>>());

        return services;
    }
}
