using MediatR;
using N5Challenge.Application.Permission.Queries.Requests;
using N5Challenge.Infrastructure.UnitOfWork.Interfaces;

namespace N5Challenge.Application.Permission.Queries.Handlers;

public class GetPermissionsHandler : IRequestHandler<GetPermissions, IEnumerable<Domain.Entities.Permissions.Permission>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Serilog.ILogger _logger;
    public GetPermissionsHandler(IUnitOfWork unitOfWork, Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<Domain.Entities.Permissions.Permission>> Handle(GetPermissions request, CancellationToken cancellationToken)
    {
        _logger.Information("Retrieving permissions data");
        return await _unitOfWork.PermissionsRepository.GetAllAsync(cancellationToken);
    }
}
