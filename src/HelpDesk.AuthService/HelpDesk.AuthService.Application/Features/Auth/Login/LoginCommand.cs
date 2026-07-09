using MediatR;

namespace HelpDesk.AuthService.Application.Features.Auth.Login;

public sealed record LoginCommand(LoginRequest Request, RequestContext Context)
    : IRequest<LoginResponse>;

public sealed record RequestContext(
    string IpAddress,
    string UserAgent);