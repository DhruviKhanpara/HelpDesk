using HelpDesk.TicketService.Domain.Common;

namespace HelpDesk.TicketService.Domain.Entities;

public class TicketCommentEntity : BaseAuditableEntity
{
    public long TicketId { get; set; }
    public TicketEntity Ticket { get; set; } = null!;

    public long UserId { get; set; }

    public string Content { get; set; } = string.Empty;

    public bool IsInternal { get; set; } = false;
}
