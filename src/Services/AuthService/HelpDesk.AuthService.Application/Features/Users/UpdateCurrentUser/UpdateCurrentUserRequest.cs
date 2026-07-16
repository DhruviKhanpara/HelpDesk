namespace HelpDesk.AuthService.Application.Features.Users.UpdateCurrentUser;

public sealed class UpdateCurrentUserRequest
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? PhoneNumber { get; init; }
}
