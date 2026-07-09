namespace HelpDesk.AuthService.Domain.Enums;

public enum LoginFailureReason
{
    None = 0,
    InvalidPassword = 1,
    UserNotFound = 2,
    AccountLocked = 3,
    AccountDisabled = 4,
    EmailNotVerified = 5,
    RefreshTokenExpired = 6
}
