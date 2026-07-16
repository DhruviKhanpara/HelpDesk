namespace HelpDesk.AuthService.Application.Features.Users.UpdateUser;

public sealed class UpdateUserRequest
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? PhoneNumber { get; init; }
    public long RoleId { get; init; }
}
