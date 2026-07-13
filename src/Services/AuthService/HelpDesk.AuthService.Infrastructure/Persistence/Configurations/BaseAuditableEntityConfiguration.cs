using HelpDesk.AuthService.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.AuthService.Infrastructure.Persistence.Configurations;

public abstract class BaseAuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseAuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedDate).IsRequired();

        builder.Property(x => x.CreatedBy).HasMaxLength(100);

        builder.Property(x => x.UpdatedBy).HasMaxLength(100);

        builder.Property(x => x.DeletedBy).HasMaxLength(100);

        builder.Property(x => x.IsDeleted)
               .HasDefaultValue(false);

        builder.HasIndex(x => x.IsDeleted);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}