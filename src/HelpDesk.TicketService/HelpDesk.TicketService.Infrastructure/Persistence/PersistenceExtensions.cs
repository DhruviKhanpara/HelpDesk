using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.TicketService.Infrastructure.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<TicketDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("TicketDbConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(TicketDbContext).Assembly.FullName));

            options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
        });

        // Application layer talks to IApplicationDbContext, never to TicketDbContext directly
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<TicketDbContext>());

        return services;
    }
}
