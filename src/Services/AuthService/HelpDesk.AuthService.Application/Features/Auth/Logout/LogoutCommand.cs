using MediatR;

namespace HelpDesk.AuthService.Application.Features.Auth.Logout;

public sealed record LogoutCommand(string RefreshToken, string? IpAddress) : IRequest;
