using HelpDesk.TicketService.Domain.Entities;

namespace HelpDesk.TicketService.Infrastructure.Persistence.SeedData;

internal static class PrioritySeed
{
    public static IEnumerable<PriorityEntity> Data => new[]
    {
        new PriorityEntity
        {
            Id = 1,
            Name = "Low",
            Description = "Low priority",
            DisplayOrder = 1,
            Color = "#28A745",
            IsActive = true
        },
        new PriorityEntity
        {
            Id = 2,
            Name = "Medium",
            Description = "Medium priority",
            DisplayOrder = 2,
            Color = "#FFC107",
            IsActive = true
        },
        new PriorityEntity
        {
            Id = 3,
            Name = "High",
            Description = "High priority",
            DisplayOrder = 3,
            Color = "#FD7E14",
            IsActive = true
        },
        new PriorityEntity
        {
            Id = 4,
            Name = "Critical",
            Description = "Critical priority",
            DisplayOrder = 4,
            Color = "#DC3545",
            IsActive = true
        }
    };
}
