using HelpDesk.TicketService.Domain.Common;

namespace HelpDesk.TicketService.Domain.Entities;

public class PriorityEntity : BaseLookupEntity
{
    public string? Color { get; set; }

    #region Navigation Section
    public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
    #endregion
}
