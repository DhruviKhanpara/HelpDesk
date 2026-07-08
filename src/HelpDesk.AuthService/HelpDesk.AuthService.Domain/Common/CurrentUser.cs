namespace HelpDesk.AuthService.Domain.Common;

public sealed class CurrentUser
{
    public long UserId { get; init; }
    public string EmployeeCode { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();

    public IReadOnlyCollection<string> Roles { get; init; } = [];

    public bool IsAuthenticated => UserId > 0;
}