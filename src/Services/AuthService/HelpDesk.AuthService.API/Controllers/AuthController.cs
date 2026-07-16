using HelpDesk.AuthService.API.Common;
using HelpDesk.AuthService.Application.Features.Auth.Login;
using HelpDesk.AuthService.Application.Features.Auth.Logout;
using HelpDesk.AuthService.Application.Features.Auth.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.AuthService.API.Controllers;

[ApiController]
[Route(ApiRoutes.V1 + "/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Authenticates a user and returns access and refresh tokens.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var command = new LoginCommand(request, new RequestContext(ipAddress, userAgent));
        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        var command = new RefreshTokenCommand(request.RefreshToken, ipAddress);
        var response = await _sender.Send(command, cancellationToken);

        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        var command = new LogoutCommand(request.RefreshToken, ipAddress);
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
