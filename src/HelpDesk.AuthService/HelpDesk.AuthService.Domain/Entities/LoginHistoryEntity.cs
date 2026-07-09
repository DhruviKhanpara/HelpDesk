using HelpDesk.AuthService.Domain.Common;
using HelpDesk.AuthService.Domain.Enums;

namespace HelpDesk.AuthService.Domain.Entities;

public class LoginHistoryEntity : BaseAuditableEntity
{
    public long? UserId { get; set; }

    public DateTime LoginDate { get; set; }
    public bool IsSuccessful { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public LoginFailureReason FailureReason { get; set; }

    #region Navigation Section
    public UserEntity? User { get; set; }
    #endregion
}
