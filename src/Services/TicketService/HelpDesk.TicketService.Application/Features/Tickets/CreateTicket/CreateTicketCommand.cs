using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;

public sealed record CreateTicketCommand(CreateTicketRequest Request) : IRequest<CreateTicketResponse>;
