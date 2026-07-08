using HelpDesk.AuthService.Domain.Common;

namespace HelpDesk.AuthService.Domain.Entities;

public class LoginHistoryEntity : BaseAuditableEntity
{
    public long UserId { get; set; }

    public DateTime LoginDate { get; set; }
    public bool IsSuccessful { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    #region Navigation Section
    public UserEntity User { get; set; } = default!;
    #endregion
}
