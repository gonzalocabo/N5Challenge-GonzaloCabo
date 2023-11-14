using N5Challenge.Infrastructure.Contexts;
using N5Challenge.Infrastructure.Elasticsearch.Interfaces;
using N5Challenge.Infrastructure.Kafka.Interfaces;
using N5Challenge.Infrastructure.Repositories.Interfaces;
using N5Challenge.Infrastructure.UnitOfWork.Interfaces;

namespace N5Challenge.Infrastructure.UnitOfWork.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly N5ChallengeDbContext _dbContext;
    private readonly IPermissionsRepository _permissionsRepository;
    private readonly IPermissionTypesRepository _permissionTypesRepository;

    public UnitOfWork(N5ChallengeDbContext dbContext, IPermissionsRepository permissionsRepository, IPermissionTypesRepository permissionTypesRepository)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(permissionsRepository);
        ArgumentNullException.ThrowIfNull(permissionTypesRepository);

        _dbContext = dbContext;
        _permissionsRepository = permissionsRepository;
        _permissionTypesRepository = permissionTypesRepository;
    }

    public IPermissionsRepository PermissionsRepository => _permissionsRepository;

    public IPermissionTypesRepository PermissionTypesRepository => _permissionTypesRepository;

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync();
    }
}
