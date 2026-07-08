using HelpDesk.TicketService.Application.Common.Interfaces;

namespace HelpDesk.TicketService.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
