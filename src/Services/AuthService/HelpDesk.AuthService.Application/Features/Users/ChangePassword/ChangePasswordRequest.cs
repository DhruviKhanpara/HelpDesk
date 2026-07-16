namespace HelpDesk.AuthService.Application.Features.Users.ChangePassword;

public sealed class ChangePasswordRequest
{
    public string CurrentPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}
