using HelpDesk.TicketService.Application.Common.Models;
using MediatR;

namespace HelpDesk.TicketService.Application.Features.Tickets.AddTicketComment;

public sealed record AddTicketCommentCommand(
    string TicketNumber,
    string Content,
    bool IsInternal) : IRequest<TicketCommentResponse>;
