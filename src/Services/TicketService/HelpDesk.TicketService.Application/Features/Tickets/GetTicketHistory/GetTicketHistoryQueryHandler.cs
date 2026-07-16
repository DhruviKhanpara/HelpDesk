using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketHistory;

public sealed class GetTicketHistoryQueryHandler : IRequestHandler<GetTicketHistoryQuery, List<TicketHistoryResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ITicketAuthorizationService _ticketAuthorization;

    public GetTicketHistoryQueryHandler(IApplicationDbContext context, ITicketAuthorizationService ticketAuthorization)
    {
        _context = context;
        _ticketAuthorization = ticketAuthorization;
    }

    public async Task<List<TicketHistoryResponse>> Handle(GetTicketHistoryQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.TicketNumber == request.TicketNumber, cancellationToken);

        if (ticket is null)
            throw new NotFoundException("Ticket", request.TicketNumber);

        _ticketAuthorization.EnsureCanView(ticket);

        return await _context.TicketHistories
            .AsNoTracking()
            .Where(x => x.TicketId == ticket.Id)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new TicketHistoryResponse
            {
                Id = x.Id,
                Action = x.Action,
                PropertyName = x.PropertyName,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
                Remarks = x.Remarks,
                PerformedBy = x.CreatedBy,
                PerformedAt = x.CreatedDate
            })
            .ToListAsync(cancellationToken);
    }
}
