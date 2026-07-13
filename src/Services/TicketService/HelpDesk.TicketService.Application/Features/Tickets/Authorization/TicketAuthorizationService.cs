using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Domain.Entities;

namespace HelpDesk.TicketService.Application.Features.Tickets.Authorization;

internal class TicketAuthorizationService : ITicketAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;

    public TicketAuthorizationService(ICurrentUserService currentUser)
    {
        _currentUserService = currentUser;
    }

    public void EnsureCanView(TicketEntity ticket)
    {
        if (CanManageTickets())
            return;

        if (ticket.RequesterUserId == _currentUserService.User.UserId)
            return;

        throw new ForbiddenException("You are not authorized to view this ticket.");
    }

    private bool CanManageTickets()
    {
        return _currentUserService.User.Roles.Any(role => role is "Admin" or "Manager" or "SupportAgent");
    }
}
