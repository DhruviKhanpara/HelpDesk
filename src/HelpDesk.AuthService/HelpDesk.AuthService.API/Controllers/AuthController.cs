using HelpDesk.AuthService.Application.Features.Login;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HelpDesk.AuthService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var command = new LoginCommand(request, new RequestContext(ipAddress, userAgent));
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
