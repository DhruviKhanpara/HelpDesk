using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.ChangePassword;

public sealed record ChangePasswordCommand(
    long UserId,
    string CurrentPassword,
    string NewPassword,
    string? IpAddress) : IRequest;
