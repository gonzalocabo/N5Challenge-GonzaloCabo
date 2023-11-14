using MediatR;
using N5Challenge.Application.Permission.Commands.Requests;
using N5Challenge.Application.Permission.Exceptions;
using N5Challenge.Infrastructure.UnitOfWork.Interfaces;
using System.Net;

namespace N5Challenge.Application.Permission.Commands.Handlers;

public class CreatePermissionHandler : IRequestHandler<CreatePermission, Domain.Entities.Permissions.Permission>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Serilog.ILogger _logger;

    public CreatePermissionHandler(IUnitOfWork unitOfWork, Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Domain.Entities.Permissions.Permission> Handle(CreatePermission request, CancellationToken cancellationToken)
    {
        _logger.Information("Handling create permission");
        if (string.IsNullOrWhiteSpace(request.EmployeeSurname) || string.IsNullOrWhiteSpace(request.EmployeeForename) || request.PermissionType <= 0)
            throw new StatusCodeException((int)HttpStatusCode.BadRequest, "Bad request to perform the operation");

        var permissionType = await _unitOfWork.PermissionTypesRepository.AnyAsync(x => x.Id == request.PermissionType);

        if(!permissionType)
            throw new StatusCodeException((int)HttpStatusCode.NotFound, "Permission type not found");

        var entity = new Domain.Entities.Permissions.Permission()
        {
            EmployeeForename = request.EmployeeForename,
            EmployeeSurname = request.EmployeeSurname,
            PermissionDate = DateTime.UtcNow,
            PermissionTypeId = request.PermissionType
        };
        
        _logger.Information("Saving added permission");
        await _unitOfWork.PermissionsRepository.Add(entity);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return entity;
    }
}
