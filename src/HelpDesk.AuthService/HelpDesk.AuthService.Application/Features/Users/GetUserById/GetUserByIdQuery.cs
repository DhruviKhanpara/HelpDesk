using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(long UserId) : IRequest<GetUserByIdResponse>;
