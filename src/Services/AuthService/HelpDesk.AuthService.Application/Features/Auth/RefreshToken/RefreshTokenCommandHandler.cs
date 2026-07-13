using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Application.Features.Auth.Login;
using HelpDesk.AuthService.Domain.Common;
using HelpDesk.AuthService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTime _dateTime;
    private readonly IRefreshTokenHasher _refreshTokenHasher;

    public RefreshTokenCommandHandler(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator, IDateTime dateTime, IRefreshTokenHasher refreshTokenHasher)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTime = dateTime;
        _refreshTokenHasher = refreshTokenHasher;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var requestTokenHash = _refreshTokenHasher.Hash(request.RefreshToken);

        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(rt => rt.TokenHash == requestTokenHash, cancellationToken);

        var now = _dateTime.Now;

        if (refreshToken is null || refreshToken.ExpiresAt <= now || refreshToken.RevokedAt is not null)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        var user = refreshToken.User;

        if (!user.IsActive)
            throw new UnauthorizedException("This account has been deactivated. Contact your administrator.");

        var roles = user.UserRoles
            .Select(x => x.Role.Name)
            .ToList();

        var jwtUser = new JwtUser
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmployeeCode = user.EmployeeCode,
            Roles = roles
        };

        var accessTokenResult = _jwtTokenGenerator.GenerateAccessToken(jwtUser);
        var refreshTokenResult = _jwtTokenGenerator.GenerateRefreshToken();
        var refreshTokenHash = _refreshTokenHasher.Hash(refreshTokenResult.Token);

        refreshToken.RevokedAt = now;
        refreshToken.RevokedByIp = request.IpAddress;
        refreshToken.ReplacedByToken = refreshTokenHash;

        var refreshTokenEntity = new RefreshTokenEntity
        {
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            ExpiresAt = refreshTokenResult.ExpiresAt,
            CreatedByIp = request.IpAddress
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessTokenResult.Token,
            RefreshToken = refreshTokenResult.Token,
            ExpiresAt = accessTokenResult.ExpiresAt,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = roles
        };
    }
}
