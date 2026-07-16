using HelpDesk.TicketService.API.Common;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Common.Models;
using HelpDesk.TicketService.Application.Features.Tickets.AddTicketComment;
using HelpDesk.TicketService.Application.Features.Tickets.AssignTicket;
using HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;
using HelpDesk.TicketService.Application.Features.Tickets.DownloadTicketAttachment;
using HelpDesk.TicketService.Application.Features.Tickets.GetTicketById;
using HelpDesk.TicketService.Application.Features.Tickets.GetTicketComments;
using HelpDesk.TicketService.Application.Features.Tickets.GetTicketHistory;
using HelpDesk.TicketService.Application.Features.Tickets.GetTickets;
using HelpDesk.TicketService.Application.Features.Tickets.UpdateTicketStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.TicketService.API.Controllers;

[ApiController]
[Route(ApiRoutes.V1 + "/tickets")]
[Authorize]
public class TicketController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IAttachmentStorageService _attachmentStorageService;

    public TicketController(ISender sender, IAttachmentStorageService attachmentStorageService)
    {
        _sender = sender;
        _attachmentStorageService = attachmentStorageService;
    }

    /// <summary>
    /// Retrieves a paginated, filterable list of tickets. Employees see only their own tickets;
    /// Admin/Manager/Support Agent see all tickets.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<TicketListItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTickets(
        [FromQuery] long? statusId,
        [FromQuery] long? priorityId,
        [FromQuery] long? categoryId,
        [FromQuery] long? assignedUserId,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTicketsQuery(
            StatusId: statusId,
            PriorityId: priorityId,
            CategoryId: categoryId,
            AssignedUserId: assignedUserId,
            Search: search,
            PageNumber: pageNumber,
            PageSize: pageSize);

        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{ticketNumber}")]
    public async Task<IActionResult> GetTicketById(string ticketNumber, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetTicketByIdQuery(ticketNumber), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{ticketNumber}/attachments/{attachmentId:int}")]
    public async Task<IActionResult> DownloadAttachment(string ticketNumber, int attachmentId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DownloadTicketAttachmentQuery(ticketNumber, attachmentId), cancellationToken);

        var stream = _attachmentStorageService.OpenRead(result.FilePath);

        return File(stream, result.ContentType, result.FileName);
    }

    /// <summary>
    /// Changes a ticket's status. Restricted to Admin, Manager, and Support Agent.
    /// </summary>
    [HttpPut("{ticketNumber}/status")]
    [ProducesResponseType(typeof(UpdateTicketStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateStatus(
        string ticketNumber, 
        [FromBody] UpdateTicketStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTicketStatusCommand(ticketNumber, request.StatusId, request.Remarks);
        var response = await _sender.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Assigns a ticket to a support user. Restricted to Admin, Manager, and Support Agent.
    /// </summary>
    [HttpPut("{ticketNumber}/assign")]
    [ProducesResponseType(typeof(AssignTicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignTicket(
        string ticketNumber,
        [FromBody] AssignTicketRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignTicketCommand(ticketNumber, request.AssignedUserId);
        var response = await _sender.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Adds a comment to a ticket. Only Admin/Manager/Support Agent may post internal comments.
    /// </summary>
    [HttpPost("{ticketNumber}/comments")]
    [ProducesResponseType(typeof(TicketCommentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddComment(
        string ticketNumber,
        [FromBody] AddTicketCommentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddTicketCommentCommand(ticketNumber, request.Content, request.IsInternal);
        var response = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetComments), new { ticketNumber }, response);
    }

    /// <summary>
    /// Retrieves comments for a ticket. Internal comments are hidden from the requester.
    /// </summary>
    [HttpGet("{ticketNumber}/comments")]
    [ProducesResponseType(typeof(List<TicketCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComments(string ticketNumber, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetTicketCommentsQuery(ticketNumber), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves the full audit history for a ticket.
    /// </summary>
    [HttpGet("{ticketNumber}/history")]
    [ProducesResponseType(typeof(List<TicketHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHistory(string ticketNumber, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetTicketHistoryQuery(ticketNumber), cancellationToken);
        return Ok(response);
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
        return CreatedAtAction(nameof(GetTicketById), new { ticketNumber = response.TicketNumber }, response);
    }
}
