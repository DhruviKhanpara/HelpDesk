using HelpDesk.TicketService.API.Common;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;
using HelpDesk.TicketService.Application.Features.Tickets.GetTicketById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpDesk.TicketService.API.Controllers;

[ApiController]
[Route("api/v1/ticket")]
[Authorize]
public class TicketController : ControllerBase
{
    private readonly ISender _sender;

    public TicketController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Creates a new support ticket.
    /// </summary>
    /// <returns>The created ticket.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTicketResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateTicket([FromForm] Contracts.Tickets.CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var tickerRequest = new CreateTicketRequest
        {
            Title = request.Title,
            Description = request.Description,
            CategoryId = request.CategoryId,

            Attachments = request.Attachments?.Select(x => (IUploadedFile)new FormUploadedFile(x)).ToList()
        };

        var command = new CreateTicketCommand(tickerRequest);
        
        var response = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTicketById), new { id = response.TicketId }, response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTicketById(long id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetTicketByIdQuery(id), cancellationToken);
        return Ok(response);
    }
}
