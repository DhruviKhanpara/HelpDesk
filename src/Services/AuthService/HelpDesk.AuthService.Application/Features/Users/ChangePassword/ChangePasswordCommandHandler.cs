using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.ChangePassword;

public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTime _dateTime;

    public ChangePasswordCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IDateTime dateTime)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _dateTime = dateTime;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(x => x.Id == request.UserId && !x.IsDeleted, cancellationToken);

        if (user is null)
            throw new NotFoundException("User", request.UserId);

        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedException("Current password is incorrect.");

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);

        var activeTokens = await _context.RefreshTokens
            .Where(x => x.UserId == user.Id && x.RevokedAt == null)
            .ToListAsync(cancellationToken);

        var now = _dateTime.Now;

        foreach (var token in activeTokens)
        {
            token.RevokedAt = now;
            token.RevokedByIp = request.IpAddress;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
