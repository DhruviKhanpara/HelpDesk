namespace HelpDesk.TicketService.Application.Common.Models;

public sealed class AttachmentUploadResult
{
    public string OriginalFileName { get; init; } = null!;
    public string StoredFileName { get; init; } = null!;
    public string RelativePath { get; init; } = null!;
    public string ContentType { get; init; } = null!;
    public long FileSize { get; init; }
}
