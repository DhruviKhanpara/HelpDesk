using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketById;

public sealed record GetTicketByIdQuery(string TicketNumber) : IRequest<GetTicketByIdResponse>;
