using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Common;
using HelpDesk.AuthService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginRequest = request.Request;

        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == loginRequest.Email, cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Your account is inactive.");

        var isPasswordValid = _passwordHasher.Verify(loginRequest.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            _context.LoginHistories.Add(new LoginHistoryEntity
            {
                UserId = user.Id,
                LoginDate = DateTime.UtcNow,
                IsSuccessful = false,
                IpAddress = request.Context.IpAddress,
                UserAgent = request.Context.UserAgent,
            });

            await _context.SaveChangesAsync(cancellationToken);

            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var roles = user.UserRoles
            .Select(x => x.Role.Name)
            .ToList();

        var jwtUser = new JwtUser
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmployeeCode = user.EmployeeCode,
            Roles = roles
        };

        var accessTokenResult = _jwtTokenGenerator.GenerateAccessToken(jwtUser);
        var refreshTokenResult = _jwtTokenGenerator.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokenEntity
        {
            UserId = user.Id,
            Token = refreshTokenResult.Token,
            ExpiresAt = refreshTokenResult.ExpiresAt,
            CreatedDate = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        user.LastLoginDate = DateTime.UtcNow;
        user.UpdatedDate = DateTime.UtcNow;

        _context.LoginHistories.Add(new LoginHistoryEntity
        {
            UserId = user.Id,
            LoginDate = DateTime.UtcNow,
            IsSuccessful = true,
            IpAddress = string.Empty,
            UserAgent = string.Empty
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessTokenResult.Token,
            RefreshToken = refreshTokenResult.Token,
            ExpiresAt = accessTokenResult.ExpiresAt,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = roles
        };
    }
}
