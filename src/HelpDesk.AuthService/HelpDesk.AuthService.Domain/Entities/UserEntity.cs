using HelpDesk.AuthService.Domain.Common;

namespace HelpDesk.AuthService.Domain.Entities;

public class UserEntity : BaseAuditableEntity
{
    public string EmployeeCode { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginDate { get; set; }

    #region Navigation Section
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
    public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
    public ICollection<LoginHistoryEntity> LoginHistory { get; set; } = new List<LoginHistoryEntity>();
    #endregion

    #region Extension methods
    public void GenerateEmployeeCode()
    {
        if (Id <= 0)
            throw new InvalidOperationException("Employee code cannot be generated before the user has been persisted.");

        EmployeeCode = $"EMP{Id:D6}";
    }
    #endregion
}
