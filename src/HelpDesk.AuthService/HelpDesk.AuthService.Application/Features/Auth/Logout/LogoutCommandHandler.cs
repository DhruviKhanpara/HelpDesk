using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Auth.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly IDateTime _dateTime;

    public LogoutCommandHandler(IApplicationDbContext context, IRefreshTokenHasher refreshTokenHasher, IDateTime dateTime)
    {
        _context = context;
        _refreshTokenHasher = refreshTokenHasher;
        _dateTime = dateTime;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = _refreshTokenHasher.Hash(request.RefreshToken);

        var refreshToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.TokenHash == tokenHash, cancellationToken);

        if (refreshToken is null)
            throw new UnauthorizedException("Invalid refresh token.");

        if (refreshToken.RevokedAt is not null)
            return;

        refreshToken.RevokedAt = _dateTime.Now;
        refreshToken.RevokedByIp = request.IpAddress;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
