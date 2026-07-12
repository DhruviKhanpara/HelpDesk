namespace HelpDesk.AuthService.Application.Features.Users.CreateUser;

public sealed class CreateUserResponse
{
    public long UserId { get; init; }
    public string EmployeeCode { get; init; } = null!;
    public string Message { get; init; } = null!;
}
