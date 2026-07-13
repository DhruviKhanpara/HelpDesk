namespace HelpDesk.TicketService.Domain.Enums;

public enum TicketStatus
{
    Open = 1,
    Assigned = 2,
    InProgress = 3,
    WaitingForUser = 4,
    Resolved = 5,
    Closed = 6,
    Cancelled = 7,
}
