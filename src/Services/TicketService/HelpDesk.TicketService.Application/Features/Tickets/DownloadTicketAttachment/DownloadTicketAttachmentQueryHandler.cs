using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.DownloadTicketAttachment;

public sealed class DownloadTicketAttachmentQueryHandler : IRequestHandler<DownloadTicketAttachmentQuery, DownloadTicketAttachmentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ITicketAuthorizationService _ticketAuthorization;

    public DownloadTicketAttachmentQueryHandler(IApplicationDbContext context, ITicketAuthorizationService ticketAuthorization)
    {
        _context = context;
        _ticketAuthorization = ticketAuthorization;
    }

    public async Task<DownloadTicketAttachmentResponse> Handle(DownloadTicketAttachmentQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Attachments.Where(x => x.Id == request.AttachmentId))
            .AsNoTracking()
            .SingleOrDefaultAsync(t => t.TicketNumber == request.TicketNumber, cancellationToken);

        if (ticket is null)
            throw new NotFoundException("Ticket not found.");

        _ticketAuthorization.EnsureCanView(ticket);

        var attachment = ticket.Attachments.SingleOrDefault(a => a.Id == request.AttachmentId);

        if (attachment is null)
            throw new NotFoundException("Attachment not found.");

        if (!File.Exists(attachment.StoragePath))
            throw new NotFoundException("Attachment file not found.");

        return new DownloadTicketAttachmentResponse
        {
            FilePath = attachment.StoragePath,
            FileName = attachment.OriginalFileName,
            ContentType = attachment.ContentType
        };
    }
}
