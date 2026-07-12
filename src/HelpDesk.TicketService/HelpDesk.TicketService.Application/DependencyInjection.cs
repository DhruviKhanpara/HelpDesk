using FluentValidation;
using HelpDesk.TicketService.Application.Features.Tickets.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HelpDesk.TicketService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ITicketAuthorizationService, TicketAuthorizationService>();

        return services;
    }
}
