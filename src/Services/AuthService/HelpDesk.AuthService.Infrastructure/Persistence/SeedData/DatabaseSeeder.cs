using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Constants;
using HelpDesk.AuthService.Domain.Entities;
using HelpDesk.AuthService.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HelpDesk.AuthService.Infrastructure.Persistence.SeedData;

internal class DatabaseSeeder
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTime _dateTime;
    private readonly SeedDataOptions _options;

    public DatabaseSeeder(IApplicationDbContext context, IPasswordHasher passwordHasher, IDateTime dateTime, IOptions<SeedDataOptions> options)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _dateTime = dateTime;
        _options = options.Value;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var adminExists = await _context.UserRoles
            .AnyAsync(x => x.Role.Name == Roles.Admin, cancellationToken);

        if (adminExists)
            return;

        var adminRole = await _context.Roles
            .FirstOrDefaultAsync(x => x.Name == Roles.Admin, cancellationToken);

        if (adminRole is null)
            throw new InvalidOperationException("Admin role has not been seeded.");

        var admin = new UserEntity
        {
            FirstName = _options.Admin.FirstName,
            LastName = _options.Admin.LastName,
            EmployeeCode = _options.Admin.EmployeeCode,
            Email = _options.Admin.Email,
            PasswordHash = _passwordHasher.Hash(_options.Admin.Password),
            IsActive = true,
            CreatedDate = _dateTime.Now,
            UserRoles = new List<UserRoleEntity>
            {
                new(adminRole.Id)
            }
        };

        _context.Users.Add(admin);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
