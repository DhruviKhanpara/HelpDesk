using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createUserRequest = request.Request;
        var email = createUserRequest.Email.Trim().ToLowerInvariant();

        var emailExists = await _context.Users
            .AnyAsync(x => x.Email == email, cancellationToken);

        if (emailExists)
            throw new ConflictException("A user with this email already exists.");

        var role = await _context.Roles
            .FirstOrDefaultAsync(x => x.Id == createUserRequest.RoleId, cancellationToken);

        if (role is null)
            throw new NotFoundException("Role", createUserRequest.RoleId);

        var passwordHash = _passwordHasher.Hash(createUserRequest.Password);

        var user = new UserEntity
        {
            FirstName = createUserRequest.FirstName,
            LastName = createUserRequest.LastName,
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true
        };

        await using var transaction = await _context.BeginTransactionAsync(cancellationToken);

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            user.GenerateEmployeeCode();

            var userRole = new UserRoleEntity(user.Id, role.Id);

            _context.UserRoles.Add(userRole);
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
