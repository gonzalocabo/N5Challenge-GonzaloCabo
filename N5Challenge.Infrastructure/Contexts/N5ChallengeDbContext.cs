using Microsoft.EntityFrameworkCore;
using N5Challenge.Domain.Entities.Permissions;

namespace N5Challenge.Infrastructure.Contexts;

public class N5ChallengeDbContext : DbContext
{
    public N5ChallengeDbContext(DbContextOptions options) : base(options)
    {
    }

    internal DbSet<Permission> Permissions { get; set; }

    internal DbSet<PermissionType> PermissionTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>()
            .HasOne(x => x.PermissionType)
            .WithMany()
            .HasForeignKey(x => x.PermissionTypeId)
            .IsRequired();
    }
}
