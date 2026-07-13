using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Domain.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HelpDesk.AuthService.Infrastructure.Authentication;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser User
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                return new CurrentUser();
            }

            return new CurrentUser
            {
                UserId = long.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0,
                EmployeeCode = user.FindFirstValue(CustomClaimTypes.EmployeeCode) ?? string.Empty,
                Email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                FirstName = user.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                LastName = user.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
                Roles = user.FindAll(ClaimTypes.Role)
                            .Select(r => r.Value)
                            .ToArray()
            };
        }
    }
}
