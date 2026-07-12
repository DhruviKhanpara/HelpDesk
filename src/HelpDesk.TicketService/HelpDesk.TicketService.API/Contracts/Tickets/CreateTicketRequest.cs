namespace HelpDesk.TicketService.API.Contracts.Tickets;

public sealed class CreateTicketRequest
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public long CategoryId { get; init; }
    public List<IFormFile>? Attachments { get; init; }
}
