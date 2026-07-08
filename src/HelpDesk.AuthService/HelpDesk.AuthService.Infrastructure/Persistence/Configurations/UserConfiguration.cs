using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.AuthService.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseAuditableEntityConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users", DbSchemas.Security);

        builder.Property(x => x.EmployeeCode)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(x => x.Email)
               .HasMaxLength(256)
               .IsRequired();

        builder.Property(x => x.PasswordHash)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(x => x.FirstName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.LastName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(x => x.EmployeeCode).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}

