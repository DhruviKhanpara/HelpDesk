using HelpDesk.AuthService.Domain.Common;

namespace HelpDesk.AuthService.Domain.Entities;

public class RefreshTokenEntity : BaseAuditableEntity
{
    public long UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByToken { get; set; }

    public string? CreatedByIp { get; set; }
    public string? RevokedByIp { get; set; }

    #region Navigation Section
    public UserEntity User { get; set; } = default!;
    #endregion
}
