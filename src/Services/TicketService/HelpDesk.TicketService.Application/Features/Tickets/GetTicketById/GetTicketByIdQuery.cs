using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketById;

public sealed record GetTicketByIdQuery(long TicketId) : IRequest<GetTicketByIdResponse>;
