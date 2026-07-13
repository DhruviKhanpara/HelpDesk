using HelpDesk.TicketService.Domain.Constants;
using HelpDesk.TicketService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.TicketService.Infrastructure.Persistence.Configurations;

public class TicketCommentConfiguration : BaseAuditableEntityConfiguration<TicketCommentEntity>
{
    public override void Configure(EntityTypeBuilder<TicketCommentEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("TicketComments", DbSchemas.Ticket);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.IsInternal)
            .HasDefaultValue(false);

        builder.HasIndex(c => c.TicketId);
        builder.HasIndex(c => c.UserId);
    }
}
