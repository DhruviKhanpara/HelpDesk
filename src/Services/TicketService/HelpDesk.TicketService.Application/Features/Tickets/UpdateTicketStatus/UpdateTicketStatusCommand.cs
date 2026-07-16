using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.UpdateTicketStatus;

public sealed record UpdateTicketStatusCommand(
    string TicketNumber,
    long StatusId,
    string? Remarks) : IRequest<UpdateTicketStatusResponse>;
