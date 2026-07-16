using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.DeactivateUser;

public sealed record DeactivateUserCommand(long UserId, string? IpAddress) : IRequest;
