using System.ComponentModel.DataAnnotations;

namespace HelpDesk.AuthService.Application.Features.Auth.RefreshToken;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; init; } = null!;
}
