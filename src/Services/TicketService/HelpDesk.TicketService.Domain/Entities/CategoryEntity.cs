using HelpDesk.TicketService.Domain.Common;

namespace HelpDesk.TicketService.Domain.Entities;

public class CategoryEntity : BaseLookupEntity
{
    public string CategoryCode { get; set; } = string.Empty;

    #region Navigation Section
    public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
    #endregion
}
