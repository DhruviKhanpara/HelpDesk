namespace HelpDesk.AuthService.Application.Features.Users.ResetPassword;

public sealed class ResetPasswordRequest
{
    public string NewPassword { get; init; } = null!;
}
