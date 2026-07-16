using HelpDesk.AuthService.Application.Features.Users.UpdateUser;
using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.UpdateCurrentUser;

public sealed record UpdateCurrentUserCommand(
    long UserId,
    string FirstName,
    string LastName,
    string? PhoneNumber) : IRequest<UpdateUserResponse>;
