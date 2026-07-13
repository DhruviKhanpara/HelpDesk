namespace HelpDesk.AuthService.Application.Features.Auth.Login;

public sealed class LoginResponse
{
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }

    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;

    public IEnumerable<string> Roles { get; init; } = [];
}
