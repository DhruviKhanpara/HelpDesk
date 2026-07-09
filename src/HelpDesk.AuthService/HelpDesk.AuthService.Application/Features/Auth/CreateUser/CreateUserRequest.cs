namespace HelpDesk.AuthService.Application.Features.Auth.CreateUser;

public sealed class CreateUserRequest
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public long RoleId { get; init; }
    public string? PhoneNumber { get; init; }
}