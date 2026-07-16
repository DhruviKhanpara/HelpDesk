using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.DeactivateUser;

public sealed class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public DeactivateUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == _currentUserService.User.UserId)
            throw new ConflictException("You cannot deactivate your own account.");

        var user = await _context.Users
            .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == request.UserId && !x.IsDeleted, cancellationToken);

        if (user is null)
            throw new NotFoundException("User", request.UserId);

        if (_currentUserService.User.Roles.Any(x => x == Roles.Manager) &&
            user.UserRoles.Any(x => x.Role.Name == Roles.Admin))
            throw new ForbiddenException("You do not have permission to access this resource.");

        if (!user.IsActive)
            throw new ConflictException("User is already deactivated.");

        user.IsActive = false;

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

