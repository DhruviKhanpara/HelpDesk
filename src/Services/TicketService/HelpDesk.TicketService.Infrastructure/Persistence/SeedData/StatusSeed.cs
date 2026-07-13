using HelpDesk.TicketService.Domain.Entities;

namespace HelpDesk.TicketService.Infrastructure.Persistence.SeedData;

internal static class StatusSeed
{
    public static IEnumerable<StatusEntity> Data => new[]
    {
        new StatusEntity
        {
            Id = 1,
            Name = "Open",
            Description = "Ticket has been created and is waiting to be assigned.",
            DisplayOrder = 1,
            IsClosedStatus = false,
            Color = "#0D6EFD",
            IsActive = true
        },
        new StatusEntity
        {
            Id = 2,
            Name = "Assigned",
            Description = "Ticket has been assigned to a support agent.",
            DisplayOrder = 2,
            IsClosedStatus = false,
            Color = "#6610F2",
            IsActive = true
        },
        new StatusEntity
        {
            Id = 3,
            Name = "In Progress",
            Description = "Support agent is actively working on the ticket.",
            DisplayOrder = 3,
            IsClosedStatus = false,
            Color = "#0DCAF0",
            IsActive = true
        },
        new StatusEntity
        {
            Id = 4,
            Name = "Waiting for User",
            Description = "Awaiting additional information or confirmation from the requester.",
            DisplayOrder = 4,
            IsClosedStatus = false,
            Color = "#FFC107",
            IsActive = true
        },
        new StatusEntity
        {
            Id = 5,
            Name = "Resolved",
            Description = "Issue has been resolved and is awaiting closure.",
            DisplayOrder = 5,
            IsClosedStatus = false,
            Color = "#198754",
            IsActive = true
        },
        new StatusEntity
        {
            Id = 6,
            Name = "Closed",
            Description = "Ticket has been completed and closed.",
            DisplayOrder = 6,
            IsClosedStatus = true,
            Color = "#6C757D",
            IsActive = true
        },
        new StatusEntity
        {
            Id = 7,
            Name = "Cancelled",
            Description = "Ticket was cancelled and will not be processed.",
            DisplayOrder = 7,
            IsClosedStatus = true,
            Color = "#DC3545",
            IsActive = true
        }
    };
}
