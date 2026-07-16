using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.ActivateUser;

public sealed class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public ActivateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(x => x.Id == request.UserId && !x.IsDeleted, cancellationToken);

        if (user is null)
            throw new NotFoundException("User", request.UserId);

        if (user.IsActive)
            throw new ConflictException("User is already active.");

        user.IsActive = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
