namespace HelpDesk.AuthService.Infrastructure.Options;

public class SeedDataOptions
{
    public const string SectionName = "SeedData";
    public AdminUserOptions Admin { get; set; } = new();
}

public class AdminUserOptions
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
}
