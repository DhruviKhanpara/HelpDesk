using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserRoles)
            .SingleOrDefaultAsync(x => x.Id == request.UserId && !x.IsDeleted, cancellationToken);

        if (user is null)
            throw new NotFoundException("User", request.UserId);

        var role = await _context.Roles
            .SingleOrDefaultAsync(x => x.Id == request.RoleId && x.IsActive, cancellationToken);

        if (role is null)
            throw new NotFoundException("Role", request.RoleId);

        if (_currentUserService.User.Roles.Any(x => x == Roles.Manager) && role.Name == Roles.Admin)
            throw new ForbiddenException("You do not have permission to access this resource.");

        var email = request.Email.Trim().ToLowerInvariant();

        if (email != user.Email)
        {
            var emailExists = await _context.Users
                .AnyAsync(x => x.Id != user.Id && x.Email == email, cancellationToken);

            if (emailExists)
                throw new ConflictException("A user with this email already exists.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = email;
        user.PhoneNumber = request.PhoneNumber;

        var currentUserRole = user.UserRoles.SingleOrDefault();

        if (currentUserRole is null || currentUserRole.RoleId != role.Id)
        {
            if (currentUserRole is not null)
                _context.UserRoles.Remove(currentUserRole);

            user.UserRoles.Add(new UserRoleEntity(user.Id, role.Id));
        }

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
            Role = role.Name
        };
    }
}
