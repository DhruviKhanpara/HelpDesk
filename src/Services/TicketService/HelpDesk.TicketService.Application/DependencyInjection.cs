using FluentValidation;
using HelpDesk.TicketService.Application.Behaviours;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HelpDesk.TicketService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>));

        services.AddScoped<ITicketAuthorizationService, TicketAuthorizationService>();

        return services;
    }
}
