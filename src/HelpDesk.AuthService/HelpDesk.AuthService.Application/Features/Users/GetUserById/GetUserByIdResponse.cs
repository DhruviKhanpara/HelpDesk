namespace HelpDesk.AuthService.Application.Features.Users.GetUserById;

public sealed class GetUserByIdResponse
{
    public long UserId { get; init; }
    public string EmployeeCode { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? PhoneNumber { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginDate { get; init; }
    public DateTime CreatedDate { get; init; }
    public List<string> Roles { get; init; } = [];
}
