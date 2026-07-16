using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using HelpDesk.TicketService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.UpdateTicketStatus;

public sealed class UpdateTicketStatusCommandHandler : IRequestHandler<UpdateTicketStatusCommand, UpdateTicketStatusResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ITicketAuthorizationService _ticketAuthorization;
    private readonly IDateTime _dateTime;

    public UpdateTicketStatusCommandHandler(IApplicationDbContext context, ITicketAuthorizationService ticketAuthorization, IDateTime dateTime)
    {
        _context = context;
        _ticketAuthorization = ticketAuthorization;
        _dateTime = dateTime;
    }

    public async Task<UpdateTicketStatusResponse> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        _ticketAuthorization.EnsureCanManage();

        var ticket = await _context.Tickets
            .Include(x => x.Status)
            .SingleOrDefaultAsync(x => x.TicketNumber == request.TicketNumber, cancellationToken);

        if (ticket is null)
            throw new NotFoundException("Ticket", request.TicketNumber);

        var newStatus = await _context.Statuses
            .SingleOrDefaultAsync(x => x.Id == request.StatusId && x.IsActive, cancellationToken);

        if (newStatus is null)
            throw new NotFoundException("Ticket status", request.StatusId);

        if (ticket.StatusId == newStatus.Id)
            throw new ConflictException($"Ticket is already in '{newStatus.Name}' status.");

        var oldStatusName = ticket.Status.Name;

        ticket.StatusId = newStatus.Id;
        ticket.ClosedDate = newStatus.IsClosedStatus ? _dateTime.Now : null;

        _context.TicketHistories.Add(new TicketHistoryEntity
        {
            TicketId = ticket.Id,
            Action = "StatusChanged",
            PropertyName = nameof(TicketEntity.StatusId),
            OldValue = oldStatusName,
            NewValue = newStatus.Name,
            Remarks = request.Remarks
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateTicketStatusResponse
        {
            TicketNumber = ticket.TicketNumber,
            StatusId = newStatus.Id,
            Status = newStatus.Name,
            ClosedDate = ticket.ClosedDate
        };
    }
}
