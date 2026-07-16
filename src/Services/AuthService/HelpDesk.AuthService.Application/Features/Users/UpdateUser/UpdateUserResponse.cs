namespace HelpDesk.AuthService.Application.Features.Users.UpdateUser;

public sealed class UpdateUserResponse
{
    public long UserId { get; init; }
    public string EmployeeCode { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? PhoneNumber { get; init; }
    public bool IsActive { get; init; }
    public string Role { get; init; } = null!;
}
