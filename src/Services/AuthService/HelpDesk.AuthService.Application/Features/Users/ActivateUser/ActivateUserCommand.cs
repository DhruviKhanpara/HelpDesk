using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.ActivateUser;

public sealed record ActivateUserCommand(long UserId) : IRequest;
