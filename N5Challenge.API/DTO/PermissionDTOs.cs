using System.ComponentModel.DataAnnotations;

namespace N5Challenge.API.DTO;

public class UpdatePermissionDTO
{
    public string? EmployeeForename { get; set; }
    public string? EmployeeSurname { get; set; }
    public int? PermissionType { get; set; }
}

public class CreatePermissionDTO
{
    [Required]
    public string EmployeeForename { get; set; }

    [Required]
    public string EmployeeSurname { get; set; }

    [Required]
    public int PermissionType { get; set; }

}