using HelpDesk.TicketService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HelpDesk.TicketService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TicketEntity> Tickets { get; }
    DbSet<TicketCommentEntity> TicketComments { get; }
    DbSet<TicketAttachmentEntity> TicketAttachments { get; }
    DbSet<TicketHistoryEntity> TicketHistories { get; }

    DbSet<CategoryEntity> Categories { get; }
    DbSet<PriorityEntity> Priorities { get; }
    DbSet<StatusEntity> Statuses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}
