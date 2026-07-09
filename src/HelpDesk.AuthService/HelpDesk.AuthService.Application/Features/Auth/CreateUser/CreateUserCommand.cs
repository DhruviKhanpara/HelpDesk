using MediatR;

namespace HelpDesk.AuthService.Application.Features.Auth.CreateUser;

public sealed record CreateUserCommand(CreateUserRequest Request)
    : IRequest<CreateUserResponse>;
