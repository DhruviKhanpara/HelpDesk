using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(CreateUserRequest Request)
    : IRequest<CreateUserResponse>;
