using HelpDesk.TicketService.Domain.Common;

namespace HelpDesk.TicketService.Domain.Entities;

public class TicketAttachmentEntity : BaseAuditableEntity
{
    public long TicketId { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string StoragePath { get; set; } = string.Empty;

    #region Navigation Section
    public TicketEntity Ticket { get; set; } = default!;
    #endregion
}
