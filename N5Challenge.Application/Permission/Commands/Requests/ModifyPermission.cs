using MediatR;

namespace N5Challenge.Application.Permission.Commands.Requests;

public class ModifyPermission : IRequest<Domain.Entities.Permissions.Permission>
{
    public int PermissionId { get; set; }
    public string? EmployeeForename { get; set; }
    public string? EmployeeSurname { get; set; }
    public int? PermissionType { get; set; }
}
