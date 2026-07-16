namespace HelpDesk.AuthService.Application.Features.Users.GetUsers;

public sealed class UserListItemResponse
{
    public long UserId { get; init; }
    public string EmployeeCode { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? PhoneNumber { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginDate { get; init; }
    public List<string> Roles { get; init; } = [];
}
