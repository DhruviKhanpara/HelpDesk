using HelpDesk.TicketService.Domain.Constants;
using HelpDesk.TicketService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.TicketService.Infrastructure.Persistence.Configurations;

public class TicketHistoryConfiguration : BaseAuditableEntityConfiguration<TicketHistoryEntity>
{
    public override void Configure(EntityTypeBuilder<TicketHistoryEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("TicketHistory", DbSchemas.Audit);

        builder.Property(x => x.Action)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PropertyName)
            .HasMaxLength(100);

        builder.Property(x => x.OldValue)
            .HasMaxLength(1000);

        builder.Property(x => x.NewValue)
            .HasMaxLength(1000);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Ticket)
            .WithMany(x => x.History)
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TicketId);

        builder.HasIndex(x => x.CreatedDate);
    }
}
