namespace HelpDesk.AuthService.Application.Features.Auth.Login;

public sealed class LoginRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
