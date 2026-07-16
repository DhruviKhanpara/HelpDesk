using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.ResetPassword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTime _dateTime;

    public ResetPasswordCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IPasswordHasher passwordHasher, IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _passwordHasher = passwordHasher;
        _dateTime = dateTime;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == request.UserId && !x.IsDeleted, cancellationToken);

        if (user is null)
            throw new NotFoundException("User", request.UserId);

        if (_currentUserService.User.Roles.Any(x => x == Roles.Manager) &&
            user.UserRoles.Any(x => x.Role.Name == Roles.Admin))
            throw new ForbiddenException("You do not have permission to access this resource.");

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);

        var activeTokens = await _context.RefreshTokens
            .Where(x => x.UserId == user.Id && x.RevokedAt == null)
            .ToListAsync(cancellationToken);

        var now = _dateTime.Now;

        foreach (var token in activeTokens)
        {
            token.RevokedAt = now;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
