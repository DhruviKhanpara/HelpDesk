namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketHistory;

public sealed class TicketHistoryResponse
{
    public long Id { get; init; }
    public string Action { get; init; } = null!;
    public string? PropertyName { get; init; }
    public string? OldValue { get; init; }
    public string? NewValue { get; init; }
    public string? Remarks { get; init; }
    public string PerformedBy { get; init; } = null!;
    public DateTime PerformedAt { get; init; }
}
