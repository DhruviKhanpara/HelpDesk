using MediatR;

namespace HelpDesk.AuthService.Application.Features.CreateUser;

public sealed record CreateUserCommand(CreateUserRequest Request)
    : IRequest<CreateUserResponse>;
