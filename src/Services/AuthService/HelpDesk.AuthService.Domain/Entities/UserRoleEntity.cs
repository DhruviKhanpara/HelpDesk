using HelpDesk.AuthService.Domain.Common;

namespace HelpDesk.AuthService.Domain.Entities;

public class UserRoleEntity : BaseAuditableEntity
{
    public UserRoleEntity(long userId, long roleId) : this(roleId)
    {
        UserId = userId;
    }

    public UserRoleEntity(long roleId)
    {
        RoleId = roleId;
    }

    public long UserId { get; set; }
    public long RoleId { get; set; }

    #region Navigation Section
    public UserEntity User { get; set; } = default!;
    public RoleEntity Role { get; set; } = default!;
    #endregion
}
