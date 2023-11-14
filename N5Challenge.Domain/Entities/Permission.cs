using N5Challenge.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace N5Challenge.Domain.Entities.Permissions;

public class Permission : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string EmployeeForename { get; set; } = string.Empty;
    public string EmployeeSurname { get; set; } = string.Empty;
    public DateTime PermissionDate { get; set; }
    [JsonIgnore, Column("PermissionType")]
    public int PermissionTypeId { get; set; }
    public virtual PermissionType? PermissionType { get; set; } = null;
}
