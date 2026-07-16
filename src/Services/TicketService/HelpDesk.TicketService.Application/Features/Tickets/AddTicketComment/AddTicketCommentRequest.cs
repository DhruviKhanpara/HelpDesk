namespace HelpDesk.TicketService.Application.Common.Models;

public sealed class AddTicketCommentRequest
{
    public string Content { get; init; } = null!;
    public bool IsInternal { get; init; }
}
