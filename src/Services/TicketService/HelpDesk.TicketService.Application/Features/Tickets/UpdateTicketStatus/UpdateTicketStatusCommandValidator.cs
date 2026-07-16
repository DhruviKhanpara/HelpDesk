using FluentValidation;

namespace HelpDesk.TicketService.Application.Features.Tickets.UpdateTicketStatus;

public sealed class UpdateTicketStatusCommandValidator : AbstractValidator<UpdateTicketStatusCommand>
{
    public UpdateTicketStatusCommandValidator()
    {
        RuleFor(x => x.TicketNumber)
            .NotEmpty();

        RuleFor(x => x.StatusId)
            .GreaterThan(0);

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}
