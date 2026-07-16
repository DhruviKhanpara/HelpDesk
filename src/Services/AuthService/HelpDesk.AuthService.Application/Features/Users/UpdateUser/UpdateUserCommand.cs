using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(
    long UserId,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    long RoleId) : IRequest<UpdateUserResponse>;
