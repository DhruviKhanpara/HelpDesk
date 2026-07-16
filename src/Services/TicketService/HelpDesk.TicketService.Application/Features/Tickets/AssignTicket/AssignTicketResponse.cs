namespace HelpDesk.TicketService.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketResponse
{
    public string TicketNumber { get; init; } = null!;
    public long AssignedUserId { get; init; }
    public long StatusId { get; init; }
    public string Status { get; init; } = null!;
}
