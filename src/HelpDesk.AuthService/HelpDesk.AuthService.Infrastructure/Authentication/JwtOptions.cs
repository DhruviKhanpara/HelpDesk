using System.ComponentModel.DataAnnotations;

namespace HelpDesk.AuthService.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    [Required]
    [MinLength(32)]
    public string Secret { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(1, 1440)]
    public int AccessTokenExpirationMinutes { get; init; }

    [Range(1, 365)]
    public int RefreshTokenExpirationDays { get; init; }
}
