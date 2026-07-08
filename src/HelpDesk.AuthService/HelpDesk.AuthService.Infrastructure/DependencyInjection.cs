using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Infrastructure.Authentication;
using HelpDesk.AuthService.Infrastructure.Persistence;
using HelpDesk.AuthService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration)
                .AddJwtAuthentication(configuration);

        services.AddSingleton<IDateTime, DateTimeService>();

        return services;
    }
}
