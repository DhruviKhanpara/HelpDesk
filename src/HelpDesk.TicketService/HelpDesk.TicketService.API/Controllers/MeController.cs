using HelpDesk.TicketService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.TicketService.API.Controllers;

[ApiController]
[Route("api/v1/me")]
[Authorize]
public class MeController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public MeController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var user = _currentUserService.User;

        return Ok(new
        {
            user.UserId,
            user.EmployeeCode,
            user.Email,
            user.FullName,
            user.Roles
        });
    }
}
