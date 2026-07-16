using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using HelpDesk.TicketService.Domain.Constants;
using HelpDesk.TicketService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandHandler : IRequestHandler<AssignTicketCommand, AssignTicketResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ITicketAuthorizationService _ticketAuthorization;

    public AssignTicketCommandHandler(IApplicationDbContext context, ITicketAuthorizationService ticketAuthorization)
    {
        _context = context;
        _ticketAuthorization = ticketAuthorization;
    }

    public async Task<AssignTicketResponse> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        _ticketAuthorization.EnsureCanManage();

        var ticket = await _context.Tickets
            .Include(x => x.Status)
            .SingleOrDefaultAsync(x => x.TicketNumber == request.TicketNumber, cancellationToken);

        if (ticket is null)
            throw new NotFoundException("Ticket", request.TicketNumber);

        if (ticket.AssignedUserId == request.AssignedUserId)
            throw new ConflictException("Ticket is already assigned to this user.");

        var previousAssignee = ticket.AssignedUserId?.ToString() ?? "Unassigned";

        ticket.AssignedUserId = request.AssignedUserId;

        _context.TicketHistories.Add(new TicketHistoryEntity
        {
            TicketId = ticket.Id,
            Action = "Assigned",
            PropertyName = nameof(TicketEntity.AssignedUserId),
            OldValue = previousAssignee,
            NewValue = request.AssignedUserId.ToString()
        });

        var statusName = ticket.Status.Name;

        if (ticket.StatusId == StatusIds.Open)
        {
            var assignedStatus = await _context.Statuses
                .SingleAsync(x => x.Id == StatusIds.Assigned, cancellationToken);

            _context.TicketHistories.Add(new TicketHistoryEntity
            {
                TicketId = ticket.Id,
                Action = "StatusChanged",
                PropertyName = nameof(TicketEntity.StatusId),
                OldValue = statusName,
                NewValue = assignedStatus.Name,
                Remarks = "Automatically updated after assignment."
            });

            ticket.StatusId = assignedStatus.Id;
            statusName = assignedStatus.Name;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new AssignTicketResponse
        {
            TicketNumber = ticket.TicketNumber,
            AssignedUserId = ticket.AssignedUserId!.Value,
            StatusId = ticket.StatusId,
            Status = statusName
        };
    }
}
