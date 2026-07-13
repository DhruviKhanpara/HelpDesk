using HelpDesk.TicketService.Application.Common.Models;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTicketById;

public sealed class GetTicketByIdResponse
{
    public long Id { get; init; }
    public string TicketNumber { get; set; } = null!;
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Category { get; init; } = null!;
    public string Status { get; init; } = null!;
    public string Priority { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
    public long RequesterUserId { get; init; }
    public IReadOnlyCollection<TicketAttachmentDto> Attachments { get; init; } = [];
}
