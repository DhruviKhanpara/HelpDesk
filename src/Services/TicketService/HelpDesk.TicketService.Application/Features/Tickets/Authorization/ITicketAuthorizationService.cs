using HelpDesk.TicketService.Domain.Entities;

namespace HelpDesk.TicketService.Application.Features.Tickets.Authorization;

public interface ITicketAuthorizationService
{
    void EnsureCanView(TicketEntity ticket);

    /// <summary>
    /// Throws <see cref="Common.Exceptions.ForbiddenException"/>
    /// unless the current user can manage tickets (status, assignment, etc.).
    /// </summary>
    void EnsureCanManage();

    /// <summary>
    /// True if the current user is Admin, Manager, or Support Agent.
    /// </summary>
    bool CanManageTickets();
}
