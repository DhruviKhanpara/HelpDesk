using FluentValidation;

namespace HelpDesk.TicketService.Application.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryValidator : AbstractValidator<GetTicketsQuery>
{
    public GetTicketsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Search)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Search));

        RuleFor(x => x.StatusId)
            .GreaterThan(0)
            .When(x => x.StatusId.HasValue);

        RuleFor(x => x.PriorityId)
            .GreaterThan(0)
            .When(x => x.PriorityId.HasValue);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .When(x => x.CategoryId.HasValue);

        RuleFor(x => x.AssignedUserId)
            .GreaterThan(0)
            .When(x => x.AssignedUserId.HasValue);
    }
}
