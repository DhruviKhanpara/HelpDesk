using HelpDesk.TicketService.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.TicketService.Domain.Entities;

public class TicketEntity : BaseAuditableEntity
{
    public string TicketNumber { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public long CategoryId { get; set; }
    public long StatusId { get; set; }
    public long PriorityId { get; set; }


    public long RequesterUserId { get; set; }
    public long? AssignedUserId { get; set; }

    public DateTime? DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    
    public string? Resolution { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;

    #region Navigation Section
    public CategoryEntity Category { get; set; } = default!;
    public StatusEntity Status { get; set; } = default!;
    public PriorityEntity Priority { get; set; } = default!;
    public ICollection<TicketCommentEntity> Comments { get; set; } = new List<TicketCommentEntity>();
    public ICollection<TicketAttachmentEntity> Attachments { get; set; } = new List<TicketAttachmentEntity>();
    public ICollection<TicketHistoryEntity> History { get; set; } = new List<TicketHistoryEntity>();
    #endregion

    #region Extension methods
    public void AssignTicketNumber()
    {
        if (Id <= 0)
            throw new InvalidOperationException("Ticket number cannot be generated before the ticket has been persisted.");

        TicketNumber = $"HD-{Id:D6}";
    }
    #endregion
}
