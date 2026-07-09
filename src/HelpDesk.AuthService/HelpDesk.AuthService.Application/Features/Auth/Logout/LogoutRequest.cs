using System.ComponentModel.DataAnnotations;

namespace HelpDesk.AuthService.Application.Features.Auth.Logout;

public sealed class LogoutRequest
{
    [Required]
    public string RefreshToken { get; init; } = null!;
}
