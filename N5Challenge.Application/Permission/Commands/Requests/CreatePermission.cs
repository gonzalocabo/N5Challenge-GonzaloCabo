using MediatR;

namespace N5Challenge.Application.Permission.Commands.Requests;

public class CreatePermission : IRequest<Domain.Entities.Permissions.Permission>
{
    public string EmployeeForename { get; set; }
    public string EmployeeSurname { get; set; }
    public int PermissionType { get; set; }
}
