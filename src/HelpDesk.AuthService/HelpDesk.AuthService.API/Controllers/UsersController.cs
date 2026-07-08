using HelpDesk.AuthService.Application.Features.CreateUser;
using HelpDesk.AuthService.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.AuthService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateUserCommand(request), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);

        //Replace with bellow once GetUserById completed
        //return CreatedAtAction(nameof(GetUserById), new { id = response.UserId }, response);
    }
}
