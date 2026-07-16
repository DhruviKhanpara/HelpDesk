namespace HelpDesk.TicketService.Application.Features.Tickets.UpdateTicketStatus;

public sealed class UpdateTicketStatusResponse
{
    public string TicketNumber { get; init; } = null!;
    public long StatusId { get; init; }
    public string Status { get; init; } = null!;
    public DateTime? ClosedDate { get; init; }
}
