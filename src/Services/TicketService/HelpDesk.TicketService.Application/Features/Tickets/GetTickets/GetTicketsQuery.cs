using HelpDesk.TicketService.Application.Common.Models;
using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTickets;

public sealed record GetTicketsQuery(
    long? StatusId,
    long? PriorityId,
    long? CategoryId,
    long? AssignedUserId,
    string? Search,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedList<TicketListItemResponse>>;
