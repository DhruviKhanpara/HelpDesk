using HelpDesk.AuthService.Domain.Common;

namespace HelpDesk.AuthService.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    TokenResult GenerateAccessToken(JwtUser user);
    TokenResult GenerateRefreshToken();
}

public sealed record TokenResult(
    string Token,
    DateTime ExpiresAt);