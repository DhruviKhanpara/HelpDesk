namespace HelpDesk.TicketService.Application.Features.Tickets.GetTickets;

public sealed class TicketListItemResponse
{
    public long Id { get; init; }
    public string TicketNumber { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Category { get; init; } = null!;
    public string Status { get; init; } = null!;
    public string Priority { get; init; } = null!;
    public long RequesterUserId { get; init; }
    public long? AssignedUserId { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime CreatedAt { get; init; }
}
