namespace HelpDesk.TicketService.Application.Common.Models;

public sealed class TicketAttachmentDto
{
    public long Id { get; init; }
    public string FileName { get; init; } = null!;
    public string ContentType { get; init; } = null!;
    public long FileSize { get; init; }
    public string Url { get; init; } = null!;
}
