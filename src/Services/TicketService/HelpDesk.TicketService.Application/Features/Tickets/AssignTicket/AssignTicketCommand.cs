using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.AssignTicket;

public sealed record AssignTicketCommand(
    string TicketNumber,
    long AssignedUserId) : IRequest<AssignTicketResponse>;
