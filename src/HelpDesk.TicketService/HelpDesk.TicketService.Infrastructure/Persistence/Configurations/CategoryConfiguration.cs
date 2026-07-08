using HelpDesk.TicketService.Domain.Constants;
using HelpDesk.TicketService.Domain.Entities;
using HelpDesk.TicketService.Infrastructure.Persistence.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.TicketService.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Categories", DbSchemas.Lookup);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CategoryCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(x => x.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(c => c.Name).IsUnique();
        builder.HasIndex(x => x.CategoryCode).IsUnique();

        builder.HasData(CategorySeed.Data);
    }
}
