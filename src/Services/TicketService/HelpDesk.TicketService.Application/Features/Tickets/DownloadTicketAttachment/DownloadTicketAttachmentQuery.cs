using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.DownloadTicketAttachment;

public sealed record DownloadTicketAttachmentQuery(string TicketNumber, int AttachmentId) : IRequest<DownloadTicketAttachmentResponse>;
