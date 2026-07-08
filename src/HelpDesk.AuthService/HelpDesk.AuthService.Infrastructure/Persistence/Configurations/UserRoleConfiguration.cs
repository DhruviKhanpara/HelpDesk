using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.AuthService.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : BaseAuditableEntityConfiguration<UserRoleEntity>
{
    public override void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserRoles", DbSchemas.Security);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.RoleId })
            .IsUnique();
    }
}
