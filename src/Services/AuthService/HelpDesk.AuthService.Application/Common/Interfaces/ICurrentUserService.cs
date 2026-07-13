using HelpDesk.AuthService.Domain.Common;

namespace HelpDesk.AuthService.Application.Common.Interfaces;

/// <summary>
/// Reads the current user's identity from the validated JWT (HttpContext claims).
/// </summary>
public interface ICurrentUserService
{
    CurrentUser User { get; }
}
