using HelpDesk.AuthService.Application.Features.Auth.Login;
using MediatR;

namespace HelpDesk.AuthService.Application.Features.Auth.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken, string? IpAddress) : IRequest<LoginResponse>;
