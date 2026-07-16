using FluentValidation;

namespace HelpDesk.TicketService.Application.Features.Tickets.AddTicketComment;

public sealed class AddTicketCommentCommandValidator : AbstractValidator<AddTicketCommentCommand>
{
    public AddTicketCommentCommandValidator()
    {
        RuleFor(x => x.TicketNumber)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(2000);
    }
}
