using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Common.Models;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketComments;

public sealed class GetTicketCommentsQueryHandler : IRequestHandler<GetTicketCommentsQuery, List<TicketCommentResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ITicketAuthorizationService _ticketAuthorization;

    public GetTicketCommentsQueryHandler(IApplicationDbContext context, ITicketAuthorizationService ticketAuthorization)
    {
        _context = context;
        _ticketAuthorization = ticketAuthorization;
    }

    public async Task<List<TicketCommentResponse>> Handle(GetTicketCommentsQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.TicketNumber == request.TicketNumber, cancellationToken);

        if (ticket is null)
            throw new NotFoundException("Ticket", request.TicketNumber);

        _ticketAuthorization.EnsureCanView(ticket);

        var canManage = _ticketAuthorization.CanManageTickets();

        return await _context.TicketComments
            .AsNoTracking()
            .Where(x => x.TicketId == ticket.Id && (canManage || !x.IsInternal))
            .OrderBy(x => x.CreatedDate)
            .Select(x => new TicketCommentResponse
            {
                Id = x.Id,
                TicketId = x.TicketId,
                UserId = x.UserId,
                Content = x.Content,
                IsInternal = x.IsInternal,
                CreatedAt = x.CreatedDate
            })
            .ToListAsync(cancellationToken);
    }
}
