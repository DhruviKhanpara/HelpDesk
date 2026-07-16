using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Common.Models;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, PaginatedList<TicketListItemResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ITicketAuthorizationService _ticketAuthorization;

    public GetTicketsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, ITicketAuthorizationService ticketAuthorization)
    {
        _context = context;
        _currentUser = currentUser;
        _ticketAuthorization = ticketAuthorization;
    }

    public async Task<PaginatedList<TicketListItemResponse>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tickets.AsNoTracking();

        if (!_ticketAuthorization.CanManageTickets())
            query = query.Where(x => x.RequesterUserId == _currentUser.User.UserId);

        if (request.StatusId.HasValue)
            query = query.Where(x => x.StatusId == request.StatusId.Value);

        if (request.PriorityId.HasValue)
            query = query.Where(x => x.PriorityId == request.PriorityId.Value);

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == request.CategoryId.Value);

        if (request.AssignedUserId.HasValue)
            query = query.Where(x => x.AssignedUserId == request.AssignedUserId.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(x => x.Title.Contains(search) || x.TicketNumber.Contains(search));
        }

        var projected = query
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new TicketListItemResponse
            {
                Id = x.Id,
                TicketNumber = x.TicketNumber,
                Title = x.Title,
                Category = x.Category.Name,
                Status = x.Status.Name,
                Priority = x.Priority.Name,
                RequesterUserId = x.RequesterUserId,
                AssignedUserId = x.AssignedUserId,
                DueDate = x.DueDate,
                CreatedAt = x.CreatedDate
            });

        return await PaginatedList<TicketListItemResponse>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}
