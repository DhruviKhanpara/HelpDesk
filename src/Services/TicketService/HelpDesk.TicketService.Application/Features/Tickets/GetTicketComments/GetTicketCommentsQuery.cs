using HelpDesk.TicketService.Application.Common.Models;
using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketComments;

public sealed record GetTicketCommentsQuery(string TicketNumber) : IRequest<List<TicketCommentResponse>>;
