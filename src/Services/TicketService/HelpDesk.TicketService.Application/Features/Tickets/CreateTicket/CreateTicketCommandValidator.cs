using FluentValidation;

namespace HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.Request.Title)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(200);

        RuleFor(x => x.Request.Description)
            .NotEmpty()
            .MaximumLength(5000);

        RuleFor(x => x.Request.CategoryId)
            .GreaterThan(0);
    }
}
