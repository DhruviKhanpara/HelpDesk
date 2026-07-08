using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;

namespace HelpDesk.AuthService.Infrastructure.Persistence.SeedData;

internal class RoleSeed
{
    public static IEnumerable<RoleEntity> Data => new[]
    {
        new RoleEntity
        {
            Id = 1,
            Name = Roles.Admin,
            Description = "System administrator with full access.",
            DisplayOrder = 1,
            IsActive = true
        },

        new RoleEntity
        {
            Id = 2,
            Name = Roles.SupportAgent,
            Description = "Support staff responsible for handling tickets.",
            DisplayOrder = 2,
            IsActive = true
        },

        new RoleEntity
        {
            Id = 3,
            Name = Roles.Manager,
            Description = "Manager responsible for Views team tickets, reports, dashboards",
            DisplayOrder = 3,
            IsActive = true
        },

        new RoleEntity
        {
            Id = 4,
            Name = Roles.Employee,
            Description = "Employee who can create and track support tickets.",
            DisplayOrder = 4,
            IsActive = true
        }
    };
}
