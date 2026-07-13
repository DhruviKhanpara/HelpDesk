using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HelpDesk.TicketService.Infrastructure.Persistence;

internal class TicketDbContext : DbContext, IApplicationDbContext
{
    public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
    {
    }

    public DbSet<TicketEntity> Tickets => Set<TicketEntity>();
    public DbSet<TicketCommentEntity> TicketComments => Set<TicketCommentEntity>();
    public DbSet<TicketAttachmentEntity> TicketAttachments => Set<TicketAttachmentEntity>();
    public DbSet<TicketHistoryEntity> TicketHistories => Set<TicketHistoryEntity>();

    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<PriorityEntity> Priorities => Set<PriorityEntity>();
    public DbSet<StatusEntity> Statuses => Set<StatusEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Picks up Entity Configuration automatically
        builder.ApplyConfigurationsFromAssembly(typeof(TicketDbContext).Assembly);

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        await Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        await Database.RollbackTransactionAsync(cancellationToken);
    }
}
