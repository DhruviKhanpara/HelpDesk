using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Common;
using HelpDesk.AuthService.Domain.Entities;
using HelpDesk.AuthService.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Auth.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTime _dateTime;
    private readonly IRefreshTokenHasher _refreshTokenHasher;

    public LoginCommandHandler(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher, IDateTime dateTime, IRefreshTokenHasher refreshTokenHasher)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _dateTime = dateTime;
        _refreshTokenHasher = refreshTokenHasher;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        var email = model.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

        var now = _dateTime.Now;

        if (user is null)
        {
            await LogFailureAsync(null, LoginFailureReason.UserNotFound, request.Context, now, cancellationToken);
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            await LogFailureAsync(user.Id, LoginFailureReason.AccountDisabled, request.Context, now, cancellationToken);
            throw new UnauthorizedException("This account has been deactivated. Contact your administrator.");
        }

        var isPasswordValid = _passwordHasher.Verify(model.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            await LogFailureAsync(user.Id, LoginFailureReason.InvalidPassword, request.Context, now, cancellationToken);
            throw new UnauthorizedException("Invalid email or password.");
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
            TokenHash = _refreshTokenHasher.Hash(refreshTokenResult.Token),
            ExpiresAt = refreshTokenResult.ExpiresAt,
            CreatedByIp = request.Context.IpAddress
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        user.LastLoginDate = now;
        user.UpdatedDate = now;

        _context.LoginHistories.Add(new LoginHistoryEntity
        {
            UserId = user.Id,
            LoginDate = now,
            IsSuccessful = true,
            IpAddress = request.Context.IpAddress,
            UserAgent = request.Context.UserAgent,
            FailureReason = LoginFailureReason.None,
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

    #region Private Section

    private async Task LogFailureAsync(long? userId, LoginFailureReason failureReason, RequestContext context, DateTime loginDate, CancellationToken cancellationToken)
    {
        _context.LoginHistories.Add(new LoginHistoryEntity
        {
            UserId = userId,
            LoginDate = loginDate,
            IsSuccessful = false,
            IpAddress = context.IpAddress,
            UserAgent = context.UserAgent,
            FailureReason = failureReason,
        });

        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion
}
