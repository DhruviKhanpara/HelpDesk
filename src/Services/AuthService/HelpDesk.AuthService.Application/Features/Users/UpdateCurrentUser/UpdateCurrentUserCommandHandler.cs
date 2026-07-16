using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Application.Features.Users.UpdateUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.UpdateCurrentUser;

public sealed class UpdateCurrentUserCommandHandler : IRequestHandler<UpdateCurrentUserCommand, UpdateUserResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateCurrentUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateUserResponse> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == request.UserId && !x.IsDeleted, cancellationToken);

        if (user is null)
            throw new NotFoundException("User", request.UserId);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateUserResponse
        {
            UserId = user.Id,
            EmployeeCode = user.EmployeeCode,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            Role = user.UserRoles.Select(x => x.Role.Name).SingleOrDefault() ?? string.Empty
        };
    }
}
