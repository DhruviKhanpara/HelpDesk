using HelpDesk.AuthService.API.Common;
using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Application.Common.Models;
using HelpDesk.AuthService.Application.Features.Users.ActivateUser;
using HelpDesk.AuthService.Application.Features.Users.ChangePassword;
using HelpDesk.AuthService.Application.Features.Users.CreateUser;
using HelpDesk.AuthService.Application.Features.Users.DeactivateUser;
using HelpDesk.AuthService.Application.Features.Users.GetUserById;
using HelpDesk.AuthService.Application.Features.Users.GetUsers;
using HelpDesk.AuthService.Application.Features.Users.ResetPassword;
using HelpDesk.AuthService.Application.Features.Users.UpdateCurrentUser;
using HelpDesk.AuthService.Application.Features.Users.UpdateUser;
using HelpDesk.AuthService.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.AuthService.API.Controllers;

[ApiController]
[Route(ApiRoutes.V1 + "/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICurrentUserService _currentUserService;

    public UsersController(ISender sender, ICurrentUserService currentUserService)
    {
        _sender = sender;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Retrieves a paginated, filterable list of users.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [ProducesResponseType(typeof(PaginatedList<UserListItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? search,
        [FromQuery] long? roleId,
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery(search, roleId, isActive, pageNumber, pageSize);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
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
        return CreatedAtAction(nameof(GetUserById), new { id = response.UserId }, response);
    }

    /// <summary>
    /// Retrieves a single user by their id.
    /// </summary>
    [HttpGet("{id:long}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [ProducesResponseType(typeof(GetUserByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Updates a user's profile and role. Managers cannot elevate a user to Admin.
    /// </summary>
    [HttpPut("{id:long}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateUser([FromRoute] long id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.RoleId);
        var response = await _sender.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Deactivates a user account. A user cannot deactivate themselves, and Managers cannot deactivate an Admin.
    /// </summary>
    [HttpPut("{id:long}/deactivate")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeactivateUser([FromRoute] long id, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        await _sender.Send(new DeactivateUserCommand(id, ipAddress), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Reactivates a previously deactivated user. Restricted to Admin.
    /// </summary>
    [HttpPut("{id:long}/activate")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActivateUser([FromRoute] long id, CancellationToken cancellationToken)
    {
        await _sender.Send(new ActivateUserCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Retrieves the currently authenticated user's own profile.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(GetUserByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetUserByIdQuery(_currentUserService.User.UserId), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Updates the currently authenticated user's own profile. Role, Email, EmployeeCode, IsActive,
    /// and Password cannot be changed through this endpoint.
    /// </summary>
    [HttpPut("me")]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCurrentUserCommand(_currentUserService.User.UserId, request.FirstName, request.LastName, request.PhoneNumber);
        var response = await _sender.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Changes the current authenticated user's own password. Revokes all active refresh tokens.
    /// </summary>
    [HttpPut("me/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        var command = new ChangePasswordCommand(_currentUserService.User.UserId, request.CurrentPassword, request.NewPassword, ipAddress);
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Resets another user's password. Restricted to Admin and Manager; Managers cannot reset an Admin's password.
    /// </summary>
    [HttpPut("{id:long}/reset-password")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword([FromRoute] long id, [FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(new ResetPasswordCommand(id, request.NewPassword), cancellationToken);
        return NoContent();
    }
}
