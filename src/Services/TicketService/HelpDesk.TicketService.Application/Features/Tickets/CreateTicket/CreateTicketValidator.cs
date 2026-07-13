using FluentValidation;

namespace HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;

public class CreateTicketValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(5000);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
    }
}
