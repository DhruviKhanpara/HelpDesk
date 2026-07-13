using HelpDesk.TicketService.Domain.Common;

namespace HelpDesk.TicketService.Domain.Entities;

public class StatusEntity : BaseLookupEntity
{
    public string? Color { get; set; }
    public bool IsClosedStatus { get; set; }

    #region Navigation Section
    public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
    #endregion
}
