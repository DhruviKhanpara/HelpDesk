using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.AuthService.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : BaseAuditableEntityConfiguration<RefreshTokenEntity>
{
    public override void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("RefreshTokens", DbSchemas.Security);

        builder.Property(x => x.Token)
            .HasMaxLength(512)
            .IsRequired();

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId);
    }
}