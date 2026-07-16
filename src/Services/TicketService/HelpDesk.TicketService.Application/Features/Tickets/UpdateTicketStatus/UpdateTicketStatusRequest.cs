namespace HelpDesk.TicketService.Application.Features.Tickets.UpdateTicketStatus;

public sealed class UpdateTicketStatusRequest
{
    public long StatusId { get; init; }
    public string? Remarks { get; init; }
}
