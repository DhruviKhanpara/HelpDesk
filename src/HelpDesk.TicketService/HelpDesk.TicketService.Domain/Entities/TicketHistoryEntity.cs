using HelpDesk.TicketService.Domain.Common;

namespace HelpDesk.TicketService.Domain.Entities;

public class TicketHistoryEntity : BaseAuditableEntity
{
    public long TicketId { get; set; }
    public string Action { get; set; } = string.Empty;

    public string? PropertyName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    public string? Remarks { get; set; }

    #region Navigation Section
    public TicketEntity Ticket { get; set; } = default!;
    #endregion
}
