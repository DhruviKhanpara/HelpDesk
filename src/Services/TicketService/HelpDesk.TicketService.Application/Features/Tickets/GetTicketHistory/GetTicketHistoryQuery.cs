using HelpDesk.TicketService.Application.Common.Models;
using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketHistory;

public sealed record GetTicketHistoryQuery(string TicketNumber) : IRequest<List<TicketHistoryResponse>>;
