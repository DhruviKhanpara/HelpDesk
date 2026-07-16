namespace HelpDesk.TicketService.Application.Common.Models;

public sealed class TicketCommentResponse
{
    public long Id { get; init; }
    public long TicketId { get; init; }
    public long UserId { get; init; }
    public string Content { get; init; } = null!;
    public bool IsInternal { get; init; }
    public DateTime CreatedAt { get; init; }
}
