using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Common.Models;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using HelpDesk.TicketService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.AddTicketComment;

public sealed class AddTicketCommentCommandHandler : IRequestHandler<AddTicketCommentCommand, TicketCommentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ITicketAuthorizationService _ticketAuthorization;

    public AddTicketCommentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, ITicketAuthorizationService ticketAuthorization)
    {
        _context = context;
        _currentUser = currentUser;
        _ticketAuthorization = ticketAuthorization;
    }

    public async Task<TicketCommentResponse> Handle(AddTicketCommentCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.TicketNumber == request.TicketNumber, cancellationToken);

        if (ticket is null)
            throw new NotFoundException("Ticket", request.TicketNumber);

        _ticketAuthorization.EnsureCanView(ticket);

        if (request.IsInternal && !_ticketAuthorization.CanManageTickets())
            throw new ForbiddenException("Only support staff can add internal comments.");

        var comment = new TicketCommentEntity
        {
            TicketId = ticket.Id,
            UserId = _currentUser.User.UserId,
            Content = request.Content.Trim(),
            IsInternal = request.IsInternal
        };

        _context.TicketComments.Add(comment);

        _context.TicketHistories.Add(new TicketHistoryEntity
        {
            TicketId = ticket.Id,
            Action = "CommentAdded",
            Remarks = request.IsInternal ? "Internal comment added." : "Comment added."
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new TicketCommentResponse
        {
            Id = comment.Id,
            TicketId = ticket.Id,
            UserId = comment.UserId,
            Content = comment.Content,
            IsInternal = comment.IsInternal,
            CreatedAt = comment.CreatedDate
        };
    }
}
