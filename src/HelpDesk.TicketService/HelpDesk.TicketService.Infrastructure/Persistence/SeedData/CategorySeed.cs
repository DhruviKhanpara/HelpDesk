using HelpDesk.TicketService.Domain.Entities;

namespace HelpDesk.TicketService.Infrastructure.Persistence.SeedData;

internal static class CategorySeed
{
    public static IEnumerable<CategoryEntity> Data => new[]
    {
        new CategoryEntity
        {
            Id = 1,
            CategoryCode = "HW",
            Name = "Hardware",
            Description = "Hardware related issues",
            DisplayOrder = 1,
            IsActive = true
        },
        new CategoryEntity
        {
            Id = 2,
            CategoryCode = "SW",
            Name = "Software",
            Description = "Software related issues",
            DisplayOrder = 2,
            IsActive = true
        },
        new CategoryEntity
        {
            Id = 3,
            CategoryCode = "NET",
            Name = "Network",
            Description = "Network and connectivity issues",
            DisplayOrder = 3,
            IsActive = true
        },
        new CategoryEntity
        {
            Id = 4,
            CategoryCode = "EMAIL",
            Name = "Email",
            Description = "Email related issues",
            DisplayOrder = 4,
            IsActive = true
        },
        new CategoryEntity
        {
            Id = 5,
            CategoryCode = "PRN",
            Name = "Printer",
            Description = "Printer issues",
            DisplayOrder = 5,
            IsActive = true
        },
        new CategoryEntity
        {
            Id = 6,
            CategoryCode = "ACC",
            Name = "Access",
            Description = "Access and permission requests",
            DisplayOrder = 6,
            IsActive = true
        },
        new CategoryEntity
        {
            Id = 7,
            CategoryCode = "OTH",
            Name = "Other",
            Description = "Miscellaneous requests",
            DisplayOrder = 7,
            IsActive = true
        }
    };
}
