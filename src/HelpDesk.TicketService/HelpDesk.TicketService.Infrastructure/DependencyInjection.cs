using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Infrastructure.Authentication;
using HelpDesk.TicketService.Infrastructure.Persistence;
using HelpDesk.TicketService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.TicketService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Interceptor and small services first - the DbContext factory below depends on them
        services.AddPersistence(configuration)
            .AddJwtAuthentication(configuration);

        services.AddSingleton<IDateTime, DateTimeService>();

        return services;
    }
}
