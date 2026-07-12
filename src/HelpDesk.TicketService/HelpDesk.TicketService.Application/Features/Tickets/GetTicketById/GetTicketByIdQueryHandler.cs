using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Common.Models;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketById;

public sealed class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, GetTicketByIdResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ITicketAuthorizationService _ticketAuthorization;

    public GetTicketByIdQueryHandler(IApplicationDbContext context, ITicketAuthorizationService ticketAuthorization)
    {
        _context = context;
        _ticketAuthorization = ticketAuthorization;
    }

    public async Task<GetTicketByIdResponse> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .Include(x => x.Attachments)
            .Include(x => x.Category)
            .Include(x => x.Priority)
            .Include(x => x.Status)
            .Where(u => u.Id == request.TicketId && !u.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        if (ticket is null)
            throw new NotFoundException("Ticket", request.TicketId);

        _ticketAuthorization.EnsureCanView(ticket);

        return new GetTicketByIdResponse
        {
            Id = request.TicketId,
            Title = ticket.Title,
            Description = ticket.Description,
            Category = ticket.Category.Name,
            Priority = ticket.Priority.Name,
            Status = ticket.Status.Name,
            RequesterUserId = ticket.RequesterUserId,
            CreatedAt = ticket.CreatedDate,
            Attachments = ticket.Attachments.Select(a => new TicketAttachmentDto
            {
                Id = a.Id,
                FileName = a.OriginalFileName,
                ContentType = a.ContentType,
                FileSize = a.FileSize,
                Url = "/" + a.StoragePath.Replace("\\", "/")
            }).ToList()
        };
    }
}
