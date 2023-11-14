using N5Challenge.Infrastructure.Repositories.Interfaces;

namespace N5Challenge.Infrastructure.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    IPermissionsRepository PermissionsRepository { get; }
    IPermissionTypesRepository PermissionTypesRepository { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
}
