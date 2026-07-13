using HelpDesk.TicketService.Domain.Entities;

namespace HelpDesk.TicketService.Application.Features.Tickets.Authorization;

public interface ITicketAuthorizationService
{
    void EnsureCanView(TicketEntity ticket);
}
