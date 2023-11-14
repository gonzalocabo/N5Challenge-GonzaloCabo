using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using N5Challenge.Domain.Entities.Interfaces;

namespace N5Challenge.Domain.Entities.Permissions;

public class PermissionType : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Description { get; set; } = null!;
}
