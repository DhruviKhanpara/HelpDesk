namespace HelpDesk.AuthService.Application.Features.CreateUser;

public sealed class CreateUserRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public long RoleId { get; init; }
}