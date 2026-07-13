namespace HelpDesk.TicketService.Application.Features.Tickets.DownloadTicketAttachment;

public class DownloadTicketAttachmentResponse
{
    public required string FilePath { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
}
