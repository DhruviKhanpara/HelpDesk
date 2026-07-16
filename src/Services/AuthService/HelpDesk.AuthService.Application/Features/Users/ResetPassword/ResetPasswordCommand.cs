using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.ResetPassword;

public sealed record ResetPasswordCommand(long UserId, string NewPassword) : IRequest;
