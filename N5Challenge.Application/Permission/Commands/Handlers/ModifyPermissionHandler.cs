using MediatR;
using N5Challenge.Application.Permission.Commands.Requests;
using N5Challenge.Application.Permission.Exceptions;
using N5Challenge.Infrastructure.UnitOfWork.Interfaces;
using System.Net;

namespace N5Challenge.Application.Permission.Commands.Handlers;

public class ModifyPermissionHandler : IRequestHandler<ModifyPermission, Domain.Entities.Permissions.Permission>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Serilog.ILogger _logger;

    public ModifyPermissionHandler(IUnitOfWork unitOfWork, Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Domain.Entities.Permissions.Permission> Handle(ModifyPermission request, CancellationToken cancellationToken)
    {
        _logger.Information("Handling modify permission");

        if (request.PermissionId <= 0 || (string.IsNullOrWhiteSpace(request.EmployeeForename) && string.IsNullOrWhiteSpace(request.EmployeeSurname) && !request.PermissionType.HasValue))
            throw new StatusCodeException((int)HttpStatusCode.BadRequest, "Bad request to perform the operation");

        var permission = await _unitOfWork.PermissionsRepository.GetAsync(x => x.Id == request.PermissionId);

        if(permission is null)
            throw new StatusCodeException((int)HttpStatusCode.NotFound, ("Permission not found"));

        if (request.PermissionType.HasValue)
        {
            var existsPermissionType = await _unitOfWork.PermissionsRepository.AnyAsync(x => x.Id == request.PermissionType.Value);

            if (!existsPermissionType)
                throw new StatusCodeException((int)HttpStatusCode.NotFound, "Permission type not found");

            permission.PermissionTypeId = request.PermissionType.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.EmployeeForename))
            permission.EmployeeForename = request.EmployeeForename;

        if (!string.IsNullOrWhiteSpace(request.EmployeeSurname))
            permission.EmployeeSurname = request.EmployeeSurname;

        permission.PermissionDate = DateTime.Now;

        _logger.Information("Saving modified permission");
        await _unitOfWork.PermissionsRepository.Update(permission);
        await _unitOfWork.SaveAsync();

        return permission;
    }
}
