using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.AuthService.Infrastructure.Persistence.Configurations;

public class LoginHistoryConfiguration : BaseAuditableEntityConfiguration<LoginHistoryEntity>
{
    public override void Configure(EntityTypeBuilder<LoginHistoryEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("LoginHistory", DbSchemas.Audit);

        builder.HasOne(x => x.User)
            .WithMany(x => x.LoginHistory)
            .HasForeignKey(x => x.UserId);

        builder.HasIndex(x => x.LoginDate);

        builder.HasIndex(x => x.UserId);
    }
}
