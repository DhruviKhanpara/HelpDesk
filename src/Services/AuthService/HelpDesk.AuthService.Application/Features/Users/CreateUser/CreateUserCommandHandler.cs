using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUserService _currentUserService;

    public CreateUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, ICurrentUserService currentUserService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _currentUserService = currentUserService;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        var email = model.Email.Trim().ToLowerInvariant();

        if (_currentUserService.User.Roles.Any(x => x == Roles.Manager) && model.RoleId == RoleIds.Admin)
            throw new ForbiddenException("You do not have permission to access this resource.");

        var emailExists = await _context.Users
            .AnyAsync(x => x.Email == email, cancellationToken);

        if (emailExists)
            throw new ConflictException("A user with this email already exists.");

        var role = await _context.Roles
            .SingleOrDefaultAsync(x => x.Id == model.RoleId && x.IsActive, cancellationToken);

        if (role is null)
            throw new NotFoundException("Role", model.RoleId);

        var passwordHash = _passwordHasher.Hash(model.Password);

        var user = new UserEntity
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = email,
            PhoneNumber = model.PhoneNumber,
            PasswordHash = passwordHash,
            IsActive = true
        };

        user.UserRoles.Add(new UserRoleEntity(role.Id));

        await using var transaction = await _context.BeginTransactionAsync(cancellationToken);

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            user.GenerateEmployeeCode();

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return new CreateUserResponse
        {
            UserId = user.Id,
            EmployeeCode = user.EmployeeCode,
            Message = "User created successfully."
        };
    }
}
