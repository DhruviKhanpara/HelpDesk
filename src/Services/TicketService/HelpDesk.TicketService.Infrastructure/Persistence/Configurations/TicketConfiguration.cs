using HelpDesk.TicketService.Domain.Constants;
using HelpDesk.TicketService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.TicketService.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : BaseAuditableEntityConfiguration<TicketEntity>
{
    public override void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Tickets", DbSchemas.Ticket);

        builder.Property(x => x.TicketNumber)
               .HasMaxLength(20)
               .IsRequired();

        builder.HasIndex(x => x.TicketNumber)
               .IsUnique();

        builder.Property(x => x.Title)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(4000)
               .IsRequired();

        builder.Property(x => x.Resolution)
               .HasMaxLength(4000);

        builder.Property(x => x.RowVersion)
               .IsRowVersion();

        builder.HasOne(x => x.Category)
               .WithMany(x => x.Tickets)
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Priority)
               .WithMany(x => x.Tickets)
               .HasForeignKey(x => x.PriorityId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
               .WithMany(x => x.Tickets)
               .HasForeignKey(x => x.StatusId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.StatusId);

        builder.HasIndex(x => x.PriorityId);

        builder.HasIndex(x => x.CategoryId);

        builder.HasIndex(x => x.RequesterUserId);

        builder.HasIndex(x => x.AssignedUserId);

        builder.HasIndex(x => x.CreatedDate);
    }
}
