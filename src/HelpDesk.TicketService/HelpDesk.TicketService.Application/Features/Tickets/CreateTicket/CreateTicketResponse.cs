using HelpDesk.TicketService.Domain.Enums;

namespace HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketResponse
{
    public long TicketId { get; init; }
    public string TicketNumber { get; init; } = null!;
    public TicketStatus Status { get; init; }
}
