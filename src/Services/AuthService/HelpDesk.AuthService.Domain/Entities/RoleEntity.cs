using HelpDesk.AuthService.Domain.Common;

namespace HelpDesk.AuthService.Domain.Entities;

public class RoleEntity : BaseLookupEntity
{
    #region Navigation Section
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
    #endregion
}
