using HelpDesk.AuthService.Application.Features.Auth.CreateUser;
using HelpDesk.AuthService.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.AuthService.API.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Creates a new User.
    /// </summary>
    /// <returns>The created User.</returns>
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateUserCommand(request), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);

        //Replace with the below once GetUserById is completed.
        //return CreatedAtAction(nameof(GetUserById), new { id = response.UserId }, response);
    }
}
