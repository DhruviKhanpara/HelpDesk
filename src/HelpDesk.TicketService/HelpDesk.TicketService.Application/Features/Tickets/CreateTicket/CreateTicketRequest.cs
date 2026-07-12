using HelpDesk.TicketService.Application.Common.Interfaces;

namespace HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketRequest
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public long CategoryId { get; init; }
    public IReadOnlyCollection<IUploadedFile>? Attachments { get; init; }
}
