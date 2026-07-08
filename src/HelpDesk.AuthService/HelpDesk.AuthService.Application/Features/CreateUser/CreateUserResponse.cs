namespace HelpDesk.AuthService.Application.Features.CreateUser;

public sealed class CreateUserResponse
{
    public long UserId { get; init; }
    public string EmployeeCode { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
