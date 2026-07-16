using FluentValidation;

namespace HelpDesk.TicketService.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandValidator : AbstractValidator<AssignTicketCommand>
{
    public AssignTicketCommandValidator()
    {
        RuleFor(x => x.TicketNumber)
            .NotEmpty();

        RuleFor(x => x.AssignedUserId)
            .GreaterThan(0);
    }
}
